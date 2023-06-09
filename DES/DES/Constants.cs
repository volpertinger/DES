﻿namespace DES.DES
{
    static class Constants
    {
        public const byte keyLength = 56;
        public const byte blockLength = 64;
        public const byte byteSize = 8;
        public const byte blockLengthInBytes = blockLength / byteSize;
        public const byte blockPartLength = blockLength / 2;
        public const byte keyPartLength = keyLength / 2;
        /// <summary>
        /// length of minor key after expanding permutation
        /// </summary>
        public const byte minorKeyExtendedLength = 48;
        /// <summary>
        /// number of encrypt rounds
        /// </summary>
        public const byte feistelRounds = 16;
        /// <summary>
        /// number of s - blocks processing for each minor key
        /// </summary>
        public const byte kernelRounds = 8;


        /// <summary>
        /// mask for left half 32-bit from 64-bit
        /// </summary>
        public const ulong blockLeftMask = 0b11111111_11111111_11111111_11111111_00000000_00000000_00000000_00000000;
        /// <summary>
        /// mask for right half 32-bit from 64-bit
        /// </summary>
        public const ulong blockRightMask = 0b00000000_00000000_00000000_00000000_11111111_11111111_11111111_11111111;

        /// <summary>
        /// mask for removing validation key bits
        /// </summary>
        public const ulong keyMask = 0b01111111_01111111_01111111_01111111_01111111_01111111_01111111_01111111;
        /// <summary>
        /// mask for key left half 28-bit from 56-bit TODO
        /// </summary>
        public const ulong keyLeftMask = keyMask & blockLeftMask;
        /// <summary>
        /// mask for key right half 28-bit from 56-bit TODO
        /// </summary>
        public const ulong keyRightMask = keyMask & blockRightMask;

        /// <summary>
        /// shift to get part for s block input
        /// </summary>
        public const byte kernelInputShift = minorKeyExtendedLength / kernelRounds;
        /// <summary>
        /// mask for s - block processing
        /// </summary>
        public const ulong kernelMask = (1 << kernelInputShift) - 1;
        /// <summary>
        /// shift for kernel result
        /// </summary>
        public const byte kernelOutputShift = blockLength / 2 / kernelRounds;

        /// <summary>
        /// left Amount of bits that separate row and column coding of S - block
        /// </summary>
        public const byte composeLeftBorderAmount = 1;
        /// <summary>
        /// right Amount of bits that separate row and column coding of S - block
        /// </summary>
        public const byte composeRightBorderAmount = 1;
        public const byte composeColSize = kernelInputShift - composeLeftBorderAmount - composeRightBorderAmount;
        public const byte composeRowSize = kernelInputShift - composeColSize;


        public static readonly List<byte> initPermutation = new()
        {
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7,
            56, 48, 40, 32, 24, 16, 8, 0,
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6
        };
        public static readonly List<byte> finalPermutation = new()
        {
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25,
            32, 0, 40, 8, 48, 16, 56, 24
        };

        /// <summary>
        /// permutation for half - block in feistel round
        /// </summary>
        public static readonly List<byte> kernelPermutation = new()
        {
            15, 6, 19, 20,
            28, 11, 27, 16,
            0, 14, 22, 25,
            4, 17, 30, 9,
            1, 7, 23, 13,
            31, 26, 2, 8,
            18, 12, 29, 5,
            21, 10, 3, 24
        };

        /// <summary>
        /// expand permutation: 32 bit -> 48 bit
        /// </summary>
        public static readonly List<byte> expandPermutation = new()
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };
        /// <summary>
        /// S blocks
        /// </summary>
        public static readonly List<List<List<byte>>> composeMatrix = new()
        {
            // S 1
            new()
            {
                new()
                {
                    14, 4, 14, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7
                },
                new()
                {
                    0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8
                },
                new()
                {
                    4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5,  0
                },
                new()
                {
                    15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13
                }
            },
            // S 2
            new()
            {
                new()
                {
                    15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10
                },
                new()
                {
                    3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5
                },
                new()
                {
                    0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15
                },
                new()
                {
                    13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9
                }
            },
            // S 3
            new()
            {
                new()
                {
                    10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8
                },
                new()
                {
                    13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1
                },
                new()
                {
                    13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7
                },
                new()
                {
                    1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12
                }
            },
            // S 4
            new()
            {
                new()
                {
                    7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15
                },
                new()
                {
                    13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9
                },
                new()
                {
                    10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4
                },
                new()
                {
                    3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14
                }
            },
            // S 5
            new()
            {
                new()
                {
                    2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9
                },
                new()
                {
                    14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6
                },
                new()
                {
                    4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14
                },
                new()
                {
                    11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3
                }
            },
            // S 6
            new()
            {
                new()
                {
                    12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11
                },
                new()
                {
                    10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8
                },
                new()
                {
                    9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6
                },
                new()
                {
                    4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13
                }
            },
            // S 7
            new()
            {
                new()
                {
                    4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1
                },
                new()
                {
                    13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6
                },
                new()
                {
                    1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2
                },
                new()
                {
                    6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12
                }
            },
            // S 8
            new()
            {
                new()
                {
                    13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7
                },
                new()
                {
                    1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2
                },
                new()
                {
                    7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8
                },
                new()
                {
                    2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
                }
            }
        };

        /// <summary>
        /// G function - key init for permutation and deleting validation bits. 64 bit -> 56 bit key.
        /// </summary>
        public static readonly List<byte> keyInitPermutation = new()
        {
            56, 48, 40, 32, 24, 16, 8,
            0, 57, 49, 41, 33, 25, 17,
            9, 1, 58, 50, 42, 34, 26,
            18, 10, 2, 59, 51, 43, 35,
            62, 54, 46, 38, 30, 22, 14,
            6, 61, 53, 45, 37, 29, 21,
            13, 5, 60, 52, 44, 36, 28,
            20, 12, 4, 27, 19, 11, 3
        };

        /// <summary>
        /// permutation for generated keys from main key
        /// </summary>
        public static readonly List<byte> keyFinalPermutation = new()
        {
            13, 16, 10, 23, 0, 4,
            2, 27, 14, 5, 20, 9,
            22, 18, 11, 3, 25, 7,
            15, 6, 26, 19, 12, 1,
            40, 51, 30, 36, 46, 54,
            29, 39, 50, 44, 32, 47,
            43, 48, 38, 55, 33, 52,
            45, 41, 49, 35, 28, 31
        };

        /// <summary>
        /// shift value for 16 key generation
        /// </summary>
        public static readonly List<byte> keyInitShift = new()
        {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };
    }
}