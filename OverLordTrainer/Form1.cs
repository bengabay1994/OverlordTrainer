using ProcessMemory;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProcessMemory.Interfaces;
using ProcessMemory.Enums;

namespace OverLordTrainer
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        private const string c_notFound = "N/A";
        private const string c_gameName = "Overlord";
        private const string c_friendlyName = "Overlord - Rising Hell";
        public ISingleProcessMemory singleProcessMemory = new SingleProcessMemory(c_gameName);

        // Offsets
        private const long c_baseOffsetAddressForAvatarEObject = 0x4BCD84;
        private List<long> c_offsetForHealthAvatarE = new List<long>() { 0x4BCD84, 0x4, 0x24, 0x198, 0x6C, 0x24, 0x78, 0x42c };
        private List<long> c_offsetForManaAvatarE = new List<long>() { 0x4BCD84, 0x4, 0x24, 0x198, 0x6C, 0x24, 0x78, 0x470 };
        private List<long> c_offsetForGoldPlayerE = new List<long>() { 0x4BCD84, 0x4, 0x24, 0x198, 0x6C, 0x24, 0x1F8 };
        private List<long> c_offsetForMaxMinionSummonBeforeAdding5 = new List<long>() { 0x4BCD84, 0x4, 0x24, 0x198, 0x6C, 0x24, 0x204 };
        private const long c_offsetOfCallingGettingLifeForceFunction = 0x25ED23;
        private const long c_offsetOfCallingGettingLifeForceFunction2 = 0x3AF4C4;
        private const long c_offsetOfGettingLifeForceFunction = 0x286960;
        private const long c_offsetOfHookLocationForOneHitKill = 0x2CD206; // will override 2 commands, test ah, 01 (3 bytes); jne ~30bytes down (2 bytes).
        private const long c_offsetOfHookLocationForDamageMultiplier = 0x2CD1FE; // will override 1 command of 6 bytes so I will add a nop.
        private const long c_offsetToEmptyTextLocationForLifeForceMultiplier = 0x2D6;
        private const long c_offsetToEmptyTextLocationForOneHitKill = 0x318; // 66 bytes after c_offsetToEmptyTextLocation
        private const long c_offsetToEmptyTextLocationForDamageMultiplier = 0x33A;

        // Statics
        private static bool isGameOn = false;
        private static int s_maxGold = 2000000000; // 2 bil
        private static IEnumerable<byte> s_lifeForceMultiplierOriginalCode1;
        private static IEnumerable<byte> s_lifeForceMultiplierOriginalCode2;
        private static IEnumerable<byte> s_OneHitKillOriginalCode;
        private static IEnumerable<byte> s_DamageMultiplierOriginalCode;


        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            Task findGameTaks = FindGameAsync();
            Task listenToKeyStrokesTask = ListenToKeyStrokesTask();

            await Task.WhenAll(findGameTaks, listenToKeyStrokesTask).ConfigureAwait(false);
        }

        private async Task FindGameAsync()
        {
            string failReason;
            while (true)
            {
                if (singleProcessMemory.IsProcessOpen())
                {
                    isGameOn = true;
                    FoundGameL.Invoke(new Action(() => FoundGameL.Text = $"{c_friendlyName} found"));
                    
                }
                else
                {
                    isGameOn = false;
                    FoundGameL.Invoke(new Action(() => FoundGameL.Text = c_notFound));
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task ListenToKeyStrokesTask() 
        {
            while (true) 
            {
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.L))
                {
                    LifeForceMultiplierCB.Checked = !LifeForceMultiplierCB.Checked;
                }
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.H))
                {
                    InfiniteHealthCB.Checked = !InfiniteHealthCB.Checked;
                }
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.M))
                {
                    InfiniteManaCB.Checked = !InfiniteManaCB.Checked;
                }
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.NumPad9))
                {
                    ManMinionsSummonB_Click(null, EventArgs.Empty);
                }
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.G))
                {
                    GoldB_Click(null, EventArgs.Empty);
                }
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.K))
                {
                    OneHitKillCB.Checked = !OneHitKillCB.Checked;
                }
                if (IsKeyPressDown(Keys.LControlKey) && IsKeyPressDown(Keys.D))
                {
                    DamageMultiplierCB.Checked = !DamageMultiplierCB.Checked;
                }
                await Task.Delay(TimeSpan.FromMilliseconds(75));
            }
        }

        private static bool IsKeyPressDown(Keys key)
        {
            return 0 != (GetAsyncKeyState(key) & 0x8000);
        }

        private void LifeForceMultiplierCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isGameOn) 
            {
                return;
            }
            if (LifeForceMultiplierCB.Checked)
            {
                int count;
                int numOfBytesToComplete = 4;
                int fillCountIndex = 3;
                int fillAddressIndex = 30;
                if (!int.TryParse(BrownMinionsTB.Text, out count))
                {
                    return;
                }

                byte[] hook1;
                byte[] hook2;
                byte[] countAsBytes = BitConverter.GetBytes(count);
                byte[] addrOfFunctionGotLifeForceAsBytes = BitConverter.GetBytes((uint)singleProcessMemory.GetAddress(c_offsetOfGettingLifeForceFunction));
                s_lifeForceMultiplierOriginalCode1 = singleProcessMemory.ReadBytesFromOffset(c_offsetOfCallingGettingLifeForceFunction, 4);
                s_lifeForceMultiplierOriginalCode2 = singleProcessMemory.ReadBytesFromOffset(c_offsetOfCallingGettingLifeForceFunction2, 4);
                byte[] shellCode = { 0x53, 0x57, 0xBF, /*enter count here*/0x00, 0x00, 0x00, 0x00, 0x83, 0xFF, 0x00, 0x74, 0x24, 0x51, 0x52, 0x8B, 0x5C, 0x24, 0x14, 0xC6, 0x83, 0x58, 0x01, 0x00, 0x00, 0x00, 0xFF, 0x74, 0x24, 0x14, 0xBB, /*enter address here*/0x00, 0x00, 0x00, 0x00, 0xFF, 0xD3, 0x83, 0xFF, 0x01, 0x74, 0x15, 0x5A, 0x59, 0x83, 0xEF, 0x01, 0xEB, 0xD7, 0x8B, 0x5C, 0x24, 0x08, 0x89, 0x5C, 0x24, 0x0C, 0x5F, 0x5B, 0x83, 0xC4, 0x04, 0xC3, 0x5B, 0x5B, 0xEB, 0xE9 };

                int shellcodeAddr = (int)singleProcessMemory.GetAddress(c_offsetToEmptyTextLocationForLifeForceMultiplier);
                int originalCode1Addr = (int)singleProcessMemory.GetAddress(c_offsetOfCallingGettingLifeForceFunction);
                int originalCode2Addr = (int)singleProcessMemory.GetAddress(c_offsetOfCallingGettingLifeForceFunction2);


                hook1 = BitConverter.GetBytes(shellcodeAddr - (originalCode1Addr + 4));
                Trace.TraceInformation($"hook1 is: {HelperUtils.GetByteArrayAsHexString(hook1)}");
                hook2 = BitConverter.GetBytes(shellcodeAddr - (originalCode2Addr + 4));
                Trace.TraceInformation($"hook2 is: {HelperUtils.GetByteArrayAsHexString(hook2)}");

                Trace.TraceInformation($"count is: {HelperUtils.GetByteArrayAsHexString(countAsBytes)}");
                Trace.TraceInformation($"address of on life force function is: {HelperUtils.GetByteArrayAsHexString(addrOfFunctionGotLifeForceAsBytes)}");
                // fill in the count that the user entered
                for (int i = 0; i < numOfBytesToComplete; ++i)
                {
                    shellCode[fillCountIndex + i] = countAsBytes[i];
                }
                // fill in the address of the function that is being called when we catch a new lifeForce
                for (int i = 0; i < numOfBytesToComplete; ++i)
                {
                    shellCode[fillAddressIndex + i] = addrOfFunctionGotLifeForceAsBytes[i];
                }

                MemoryProtection oldProt;

                bool isChangeSucceeded = singleProcessMemory.ChangeMemoryProtection(c_offsetToEmptyTextLocationForLifeForceMultiplier, shellCode.Length, MemoryProtection.PageExecuteReadWrite, out oldProt);
                if (!isChangeSucceeded) 
                {
                    Trace.TraceWarning($"failed to change memory protection for address at offset: {c_offsetToEmptyTextLocationForLifeForceMultiplier.ToString("X")}");
                }
                Trace.TraceInformation($"old protection of shell code location was: {oldProt}");

                singleProcessMemory.WriteBytesToOffset(c_offsetToEmptyTextLocationForLifeForceMultiplier, shellCode);
                singleProcessMemory.WriteBytesToOffset(c_offsetOfCallingGettingLifeForceFunction, hook1);
                singleProcessMemory.WriteBytesToOffset(c_offsetOfCallingGettingLifeForceFunction2, hook2);
            }
            else 
            {
                if (s_lifeForceMultiplierOriginalCode1 != null && s_lifeForceMultiplierOriginalCode2 != null)
                {
                    singleProcessMemory.WriteBytesToOffset(c_offsetOfCallingGettingLifeForceFunction, s_lifeForceMultiplierOriginalCode1);
                    singleProcessMemory.WriteBytesToOffset(c_offsetOfCallingGettingLifeForceFunction2, s_lifeForceMultiplierOriginalCode2);
                }
                else 
                {
                    Trace.TraceError($"Life Force Multiplier has been unchecked but original codes are not initialized!!. orig1: {s_lifeForceMultiplierOriginalCode1}, orig2: {s_lifeForceMultiplierOriginalCode2}");
                }
            }
            // The shell code is:
            /*
                push ebx
                push edi
                mov edi, <count> // count is the number of minions to get.
                bloop:
                cmp edi, 0
                je end
                push ecx
                push edx
                mov ebx, [esp+0x14]
                mov BYTE PTR [ebx+0x158], 0
                push [esp+0x14]
                mov ebx, <Addr that is getting called on life force recive>
                call ebx
                cmp edi, 1
                je noRestore
                pop edx
                pop ecx
                endLoop:
                sub edi, 1
                jmp bloop
                end:
                mov ebx, [esp+8]
                mov [esp+0x0C], ebx                                                                                                                                                                           
                pop edi
                pop ebx
                add esp, 4
                ret
                noRestore:
                pop ebx
                pop ebx
                jmp endLoop
             */
        }

        private void InfiniteHealthCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isGameOn)
            {
                return;
            }
            if (InfiniteHealthCB.Checked)
            {
                singleProcessMemory.FreezeValue(c_offsetForHealthAvatarE, 500.0f);
            }
            else 
            {
                singleProcessMemory.UnFreezeValue(c_offsetForHealthAvatarE);
            }
        }

        private void InfiniteManaCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isGameOn)
            {
                return;
            }
            if (InfiniteManaCB.Checked)
            {
                singleProcessMemory.FreezeValue(c_offsetForManaAvatarE, 500.0f);
            }
            else
            {
                singleProcessMemory.UnFreezeValue(c_offsetForManaAvatarE);
            }
        }

        private void GoldB_Click(object sender, EventArgs e)
        {
            if (!isGameOn) 
            {
                return;
            }
            
            int gold;
            if (!int.TryParse(GetGoldTB.Text, out gold))
            {
                gold = s_maxGold;
            }
            singleProcessMemory.WriteIntToOffsets(c_offsetForGoldPlayerE, gold);
        }

        private void ManMinionsSummonB_Click(object sender, EventArgs e)
        {
            if (!isGameOn)
            {
                return;
            }

            int maxMinionToSummon;
            if (!int.TryParse(MaxMinionSummonTB.Text, out maxMinionToSummon))
            {
                return;
            }
            maxMinionToSummon -= 5;
            singleProcessMemory.WriteIntToOffsets(c_offsetForMaxMinionSummonBeforeAdding5, maxMinionToSummon);
        }

        private void OneHitKillCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isGameOn) 
            {
                return;
            }
            if (OneHitKillCB.Checked)
            {
                int address1Index = 16;
                int address2Index = 25;
                int address3Index = 30;
                int numOfBytesToComplete = 4;

                byte[] shellCode = { 0x51, 0x8B, 0x4E, 0x08, 0x8B, 0x89, 0xA4, 0x01, 0x00, 0x00, 0x83, 0xF9, 0x03, 0x59, 0x0F, 0x85, /*address1*/0x00, 0x00, 0x00, 0x00, 0xF6, 0xC4, 0x01, 0x0F, 0x85, /*address2*/0x00, 0x00, 0x00, 0x00, 0xE9, /*address3*/0x00, 0x00, 0x00, 0x00 };

                byte[] hook = { 0xE9 };

                s_OneHitKillOriginalCode = singleProcessMemory.ReadBytesFromOffset(c_offsetOfHookLocationForOneHitKill, 5);
        
                int shellcodeAddr = (int)singleProcessMemory.GetAddress(c_offsetToEmptyTextLocationForOneHitKill);
                int originalCodeAddr = (int)singleProcessMemory.GetAddress(c_offsetOfHookLocationForOneHitKill);

                // JUMP TO KILL THE ENEMY IS originalCodeAddr+5 
                hook = hook.Concat(BitConverter.GetBytes(shellcodeAddr - (originalCodeAddr + 5))).ToArray();
                Trace.TraceInformation($"hook is: {HelperUtils.GetByteArrayAsHexString(hook)}");

                byte[] address1 = BitConverter.GetBytes(originalCodeAddr + 1 - (shellcodeAddr + address1Index));
                byte[] address2 = BitConverter.GetBytes(originalCodeAddr + 1 + 35 - (shellcodeAddr + address2Index));
                byte[] address3 = BitConverter.GetBytes(originalCodeAddr + 1 - (shellcodeAddr + address3Index));

                Trace.TraceInformation($"address1 bytes are: {HelperUtils.GetByteArrayAsHexString(address1)}");
                Trace.TraceInformation($"address2 bytes are: {HelperUtils.GetByteArrayAsHexString(address2)}");
                Trace.TraceInformation($"address3 bytes are: {HelperUtils.GetByteArrayAsHexString(address3)}");

                // fill in the address in the code in my function
                for (int i = 0; i < numOfBytesToComplete; ++i)
                {
                    shellCode[address1Index + i] = address1[i];
                }
                for (int i = 0; i < numOfBytesToComplete; ++i)
                {
                    shellCode[address2Index + i] = address2[i];
                }
                for (int i = 0; i < numOfBytesToComplete; ++i)
                {
                    shellCode[address3Index + i] = address3[i];
                }

                MemoryProtection oldProt;

                bool isChangeProtectionSucceeded = singleProcessMemory.ChangeMemoryProtection(c_offsetToEmptyTextLocationForOneHitKill, shellCode.Length, MemoryProtection.PageExecuteReadWrite, out oldProt);
                if (!isChangeProtectionSucceeded) 
                {
                    Trace.TraceWarning($"failed to change memory protection for address at offset: {c_offsetToEmptyTextLocationForLifeForceMultiplier.ToString("X")}");
                }
                Trace.TraceInformation($"old protection of shell code location was: {oldProt}");

                singleProcessMemory.WriteBytesToOffset(c_offsetToEmptyTextLocationForOneHitKill, shellCode);
                singleProcessMemory.WriteBytesToOffset(c_offsetOfHookLocationForOneHitKill, hook);
            }
            else 
            {
                if (s_OneHitKillOriginalCode != null)
                {
                    Trace.TraceInformation($"Writing bytes: {HelperUtils.GetByteArrayAsHexString(s_OneHitKillOriginalCode)} to address at offset: {c_offsetOfHookLocationForOneHitKill}");
                    singleProcessMemory.WriteBytesToOffset(c_offsetOfHookLocationForOneHitKill, s_OneHitKillOriginalCode);
                }
                else
                {
                    Trace.TraceError($"One hit kill has been unchecked but original code is not initialized!!");
                }
            }
            // The shell code is:
            /*
               push ecx
               mov ecx, [esi+0x08]
               mov ecx, [ecx+0x1a4]
               cmp ecx, 0x3
               pop ecx
               jne 0x2ccee3
               test ah, 0x1
               jne 0x2ccefd
               jmp 0x2cced5
             */
        }

        private void DamageMultiplierB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isGameOn)
            {
                return;
            }
            if (DamageMultiplierCB.Checked)
            {
                int damage;
                if (!int.TryParse(DamageMultiplierTB.Text, out damage))
                {
                    damage = 2;
                }
                if ((uint)damage > 255) 
                {
                    damage = 255;
                }

                byte[] shellCode = { 0x51, 0x8B, 0x4E, 0x08, 0x8B, 0x89, 0xA4, 0x01, 0x00, 0x00, 0x83, 0xF9, 0x03, 0x74, 0x10, 0x83, 0xEC, 0x04, 0xD9, 0x1C, 0x24, 0x6A, 0x02, 0xDB, 0x04, 0x24, 0x59, 0xD8, 0x0C, 0x24, 0x59, 0x59, 0xD8, 0x96, 0xE4, 0x00, 0x00, 0x00, 0xE9, 0xC6, 0xCE, 0x2C, 0x00 };
                byte[] hook = { 0xE9 };

                int addressIndex = shellCode.Length - 4;
                int damageIndex = 22;
                int numOfBytesToComplete = 4;

                s_DamageMultiplierOriginalCode = singleProcessMemory.ReadBytesFromOffset(c_offsetOfHookLocationForDamageMultiplier, 6);

                int shellcodeAddr = (int)singleProcessMemory.GetAddress(c_offsetToEmptyTextLocationForDamageMultiplier);
                int originalCodeAddr = (int)singleProcessMemory.GetAddress(c_offsetOfHookLocationForDamageMultiplier);

                // JUMP TO KILL THE ENEMY IS originalCodeAddr+5 
                hook = hook.Concat(BitConverter.GetBytes(shellcodeAddr - (originalCodeAddr + 5))).Concat(new byte[] { 0x90 }).ToArray();
                Trace.TraceInformation($"hook is: {HelperUtils.GetByteArrayAsHexString(hook)}");

                byte[] address = BitConverter.GetBytes(originalCodeAddr + 2 - (shellcodeAddr + addressIndex));

                Trace.TraceInformation($"address bytes are: {HelperUtils.GetByteArrayAsHexString(address)}");

                // fill in the address in the code in my function
                for (int i = 0; i < numOfBytesToComplete; ++i)
                {
                    shellCode[addressIndex + i] = address[i];
                }
                shellCode[damageIndex] = Convert.ToByte(damage); // fill in the damge multiplier

                MemoryProtection oldProt;

                singleProcessMemory.ChangeMemoryProtection(c_offsetToEmptyTextLocationForDamageMultiplier, shellCode.Length ,MemoryProtection.PageExecuteReadWrite, out oldProt);
                Trace.TraceInformation($"old protection of shell code location was: {oldProt}");

                singleProcessMemory.WriteBytesToOffset(c_offsetToEmptyTextLocationForDamageMultiplier, shellCode);
                singleProcessMemory.WriteBytesToOffset(c_offsetOfHookLocationForDamageMultiplier, hook);
            }
            else
            {
                if (s_DamageMultiplierOriginalCode != null)
                {
                    Trace.TraceInformation($"Writing bytes: {HelperUtils.GetByteArrayAsHexString(s_DamageMultiplierOriginalCode)} to address at offset: {c_offsetOfHookLocationForDamageMultiplier}");
                    singleProcessMemory.WriteBytesToOffset(c_offsetOfHookLocationForDamageMultiplier, s_DamageMultiplierOriginalCode);
                }
                else
                {
                    Trace.TraceError($"One hit kill has been unchecked but original code is not initialized!!");
                }
            }
            // The shell code is:
            /*
                push ecx
                mov ecx, [esi+0x08]
                mov ecx, [ecx+0x1a4]
                cmp ecx, 0x3
                je exit
                push 0x2
                mov ecx, esp
                fmul dword ptr [ecx]
                pop ecx
                exit: 
                pop ecx
                fcom DWORD PTR [esi+0xE4]
                jmp toContinueTheOriginalCode
             */
        }
    }
}
