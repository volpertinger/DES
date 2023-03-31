using DES;

namespace UnitTests
{
    [TestClass]
    public class BitUtilsTest
    {
        private const byte maxBitIndex = 63;
        private const byte maxBitAmount = maxBitIndex + 1;
        [TestMethod]
        public void GetBit()
        {
            for (byte i = 0; i <= maxBitIndex; ++i)
            {
                for (byte j = 0; j <= maxBitIndex; ++j)
                {
                    if (j == i)
                    {
                        Assert.AreEqual(true, BitUtils.GetBit(1ul << i, j));
                    }
                    else
                    {
                        Assert.AreEqual(false, BitUtils.GetBit(1ul << i, j));
                    }
                }
            }
        }
        [TestMethod]
        public void SetBit()
        {
            for (byte i = 0; i < maxBitIndex; ++i)
            {
                var number = 1ul << i;
                // 0 -> 1
                Assert.AreEqual(number, BitUtils.SetBit(0, i));
                Assert.AreEqual(number, BitUtils.SetBit(0, i, true));
                // 1 -> 1
                Assert.AreEqual(number, BitUtils.SetBit(number, i));
                Assert.AreEqual(number, BitUtils.SetBit(number, i, true));
                // 1 -> 0
                Assert.AreEqual(0u, BitUtils.SetBit(number, i, false));
            }
        }

        [TestMethod]
        public void ClearBit()
        {
            // 1 -> 0
            Assert.AreEqual(1u, BitUtils.ClearBit(3, 1));
            Assert.AreEqual(0u, BitUtils.ClearBit(1, 0));
            Assert.AreEqual(16u, BitUtils.ClearBit(24, 3));
            Assert.AreEqual(0u, BitUtils.ClearBit(1ul << maxBitIndex, maxBitIndex));
            // 0 -> 0
            Assert.AreEqual(3u, BitUtils.ClearBit(3, 2));
            Assert.AreEqual(1u, BitUtils.ClearBit(1, 1));
            Assert.AreEqual(24u, BitUtils.ClearBit(24, 5));
            Assert.AreEqual(1ul << maxBitIndex, BitUtils.ClearBit(1ul << maxBitIndex, 30));
        }

        [TestMethod]
        public void SetFirst()
        {
            var number = 1ul + (1ul << 1) + (1ul << 2);
            Assert.AreEqual(number, BitUtils.SetFirstBits(0, 3));
            Assert.AreEqual(number, BitUtils.SetFirstBits(number, 3));
        }

        [TestMethod]
        public void ClearFirst()
        {
            var number = 1ul + (1ul << 1) + (1ul << 2);
            Assert.AreEqual(0u, BitUtils.ClearFirstBits(number, 3));
            Assert.AreEqual(0u, BitUtils.ClearFirstBits(number, 18));
            Assert.AreEqual(1ul << 2, BitUtils.ClearFirstBits(number, 2));
        }

        [TestMethod]
        public void BitsSwitch()
        {
            for (byte i = 0; i < maxBitIndex; ++i)
            {
                var numberBefore = 1ul << i;
                byte otherIndex = (byte)(maxBitIndex - i);
                var numberAfter = 1ul << otherIndex;
                Assert.AreEqual(numberAfter, BitUtils.BitsSwitch(numberBefore, i, otherIndex));
            }
        }

        [TestMethod]
        public void OuterJoin()
        {
            Assert.AreEqual(0b0011u, BitUtils.OuterJoin(0b000111, 2, 2, 6));
            Assert.AreEqual(0b0u, BitUtils.OuterJoin(0b000111, 0, 0, 6));
            Assert.AreEqual(0b000111u, BitUtils.OuterJoin(0b000111, 4, 2, 6));
            Assert.AreEqual(0b1101u, BitUtils.OuterJoin(0b110000110101, 2, 2, 12));
            Assert.AreEqual(0b110001u, BitUtils.OuterJoin(0b110000110101, 2, 4, 12));
        }

        [TestMethod]
        public void InnerJoin()
        {
            Assert.AreEqual(0b0u, BitUtils.InnerJoin(0b010101, 3, 3, 6));
            Assert.AreEqual(0b010101u, BitUtils.InnerJoin(0b010101, 0, 0, 6));
            Assert.AreEqual(0b0101u, BitUtils.InnerJoin(0b010101, 2, 0, 6));
            Assert.AreEqual(0b101u, BitUtils.InnerJoin(0b010101, 0, 3, 6));
            Assert.AreEqual(0b1u, BitUtils.InnerJoin(0b011111010101, 6, 5, 12));
            Assert.AreEqual(0b111101010u, BitUtils.InnerJoin(0b011111010101, 1, 2, 12));
        }

        [TestMethod]
        public void GetByte()
        {
            var byte_0 = 0b00110000u;
            var byte_1 = 0b11001111u;
            var byte_2 = 0b00001111u;
            var byte_3 = 0b11110000u;
            var number = (byte_3 << 24) + (byte_2 << 16) + (byte_1 << 8) + byte_0;
            Assert.AreEqual((byte)byte_0, BitUtils.GetByte(number, 0));
            Assert.AreEqual((byte)byte_1, BitUtils.GetByte(number, 1));
            Assert.AreEqual((byte)byte_2, BitUtils.GetByte(number, 2));
            Assert.AreEqual((byte)byte_3, BitUtils.GetByte(number, 3));
        }

        [TestMethod]
        public void SetByte()
        {
            var byte_0 = 0b00110000u;
            var byte_1 = 0b11001111u;
            var byte_2 = 0b00001111u;
            var byte_3 = 0b11110000u;
            var number = (byte_3 << 24) + (byte_2 << 16) + (byte_1 << 8) + byte_0;
            // immutable test
            Assert.AreEqual(number, BitUtils.SetByte(number, 0, (byte)byte_0));
            Assert.AreEqual(number, BitUtils.SetByte(number, 1, (byte)byte_1));
            Assert.AreEqual(number, BitUtils.SetByte(number, 2, (byte)byte_2));
            Assert.AreEqual(number, BitUtils.SetByte(number, 3, (byte)byte_3));
            // mutable tests
            Assert.AreEqual(number - byte_0 + byte_1, BitUtils.SetByte(number, 0, (byte)byte_1));
            Assert.AreEqual(number - byte_0 + byte_2, BitUtils.SetByte(number, 0, (byte)byte_2));
            Assert.AreEqual(number - byte_0 + byte_3, BitUtils.SetByte(number, 0, (byte)byte_3));
            Assert.AreEqual(number - (byte_1 << 8) + (byte_0 << 8), BitUtils.SetByte(number, 1, (byte)byte_0));
            Assert.AreEqual(number - (byte_1 << 8) + (byte_2 << 8), BitUtils.SetByte(number, 1, (byte)byte_2));
            Assert.AreEqual(number - (byte_1 << 8) + (byte_3 << 8), BitUtils.SetByte(number, 1, (byte)byte_3));
            Assert.AreEqual(number - (byte_2 << 16) + (byte_0 << 16), BitUtils.SetByte(number, 2, (byte)byte_0));
            Assert.AreEqual(number - (byte_2 << 16) + (byte_1 << 16), BitUtils.SetByte(number, 2, (byte)byte_1));
            Assert.AreEqual(number - (byte_2 << 16) + (byte_3 << 16), BitUtils.SetByte(number, 2, (byte)byte_3));
            Assert.AreEqual(number - (byte_3 << 24) + (byte_0 << 24), BitUtils.SetByte(number, 3, (byte)byte_0));
            Assert.AreEqual(number - (byte_3 << 24) + (byte_1 << 24), BitUtils.SetByte(number, 3, (byte)byte_1));
            Assert.AreEqual(number - (byte_3 << 24) + (byte_2 << 24), BitUtils.SetByte(number, 3, (byte)byte_2));
        }

        [TestMethod]
        public void BytesSwitch()
        {
            var byte_0 = 0b00110000u;
            var byte_1 = 0b11001111u;
            var byte_2 = 0b00001111u;
            var byte_3 = 0b11110000u;
            var number = (byte_3 << 24) + (byte_2 << 16) + (byte_1 << 8) + byte_0;
            // immutable test
            Assert.AreEqual(number, BitUtils.BytesSwitch(number, 0, 0));
            Assert.AreEqual(number, BitUtils.BytesSwitch(number, 1, 1));
            Assert.AreEqual(number, BitUtils.BytesSwitch(number, 2, 2));
            Assert.AreEqual(number, BitUtils.BytesSwitch(number, 3, 3));
            // mutable test
            var switched = number - byte_0 + byte_1 - (byte_1 << 8) + (byte_0 << 8);
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 0, 1));
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 1, 0));

            switched = number - byte_0 + byte_2 - (byte_2 << 16) + (byte_0 << 16);
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 0, 2));
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 2, 0));

            switched = number - byte_0 + byte_3 - (byte_3 << 24) + (byte_0 << 24);
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 0, 3));
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 3, 0));

            switched = number - (byte_1 << 8) + (byte_2 << 8) - (byte_2 << 16) + (byte_1 << 16);
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 1, 2));
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 2, 1));

            switched = number - (byte_1 << 8) + (byte_3 << 8) - (byte_3 << 24) + (byte_1 << 24);
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 1, 3));
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 3, 1));

            switched = number - (byte_2 << 16) + (byte_3 << 16) - (byte_3 << 24) + (byte_2 << 24);
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 2, 3));
            Assert.AreEqual(switched, BitUtils.BytesSwitch(number, 3, 2));
        }

        [TestMethod]
        public void MaxPower()
        {
            Assert.AreEqual(maxBitIndex, BitUtils.MaxNumberPower2(0));
            for (int i = 0; i < maxBitAmount; ++i)
            {
                Assert.AreEqual(1ul << i, BitUtils.MaxNumberPower2(1ul << i));
                if (i != 0)
                    Assert.AreEqual(1u, BitUtils.MaxNumberPower2((1ul << i) - 1));
            }
            Assert.AreEqual(1ul, BitUtils.MaxNumberPower2(0b000001));
            Assert.AreEqual(1ul, BitUtils.MaxNumberPower2(0b1110101010100101));
            Assert.AreEqual(1ul, BitUtils.MaxNumberPower2(0b1010100010100101));
            Assert.AreEqual(1ul, BitUtils.MaxNumberPower2(0b01001010101010101010101010111111));
            Assert.AreEqual(1ul << 2, BitUtils.MaxNumberPower2(0b00000000000100));
            Assert.AreEqual(1ul << 5, BitUtils.MaxNumberPower2(0b000010101010101100000));
            Assert.AreEqual(1ul << 30, BitUtils.MaxNumberPower2(0b11000000000000000000000000000000));
            Assert.AreEqual(1ul << 29, BitUtils.MaxNumberPower2(0b10100000000000000000000000000000));
            Assert.AreEqual(1ul << 28, BitUtils.MaxNumberPower2(0b10010000000000000000000000000000));
            Assert.AreEqual(1ul << 27, BitUtils.MaxNumberPower2(0b10001000000000000000000000000000));
            Assert.AreEqual(1ul << 26, BitUtils.MaxNumberPower2(0b10000100000000000000000000000000));
        }

        [TestMethod]
        public void CyclicShift()
        {
            // basic shift
            for (byte i = 0; i <= maxBitIndex; ++i)
            {
                Assert.AreEqual(1ul << i, BitUtils.CyclicShiftLeft(1u, i));
                var number = 1ul << maxBitIndex;
                Assert.AreEqual(number >> i, BitUtils.CyclicShiftRight(number, i));
                // immutability
                Assert.AreEqual(number, BitUtils.CyclicShiftRight(number, maxBitAmount));
                Assert.AreEqual(number, BitUtils.CyclicShiftLeft(number, maxBitAmount));

            }
            // cyclic shift right

            Assert.AreEqual(0b10000000_00000000_00000000_00000000u,
                BitUtils.CyclicShiftRight(0b00000000_00000000_00000000_00000001, 1, 32));

            Assert.AreEqual(0b00000000_00000000_00000000_00000010u,
                BitUtils.CyclicShiftRight(0b00000000_00000000_00000000_00000001, 31, 32));

            Assert.AreEqual(0b00011111_11100000_00011111_11100000u,
                BitUtils.CyclicShiftRight(0b11111111_00000000_11111111_00000000ul, 3, 32));

            Assert.AreEqual(0b10101010_10101010_10101010_10101010u,
                BitUtils.CyclicShiftRight(0b10101010_10101010_10101010_10101010, 4, 32));

            Assert.AreEqual(0b01010101_01010101_01010101_01010101u,
                BitUtils.CyclicShiftRight(0b10101010_10101010_10101010_10101010, 3, 32));

            Assert.AreEqual(0b01010101_01010101_01000000_00010101u,
                BitUtils.CyclicShiftRight(0b10101010_10101010_00000000_10101010, 3, 32));

            Assert.AreEqual(0b00000000_11111111_00000000_11111111_00000000_11111111_00000000_11111111ul,
                BitUtils.CyclicShiftRight(0b11111111_00000000_11111111_00000000_11111111_00000000_11111111_00000000ul, 8));

            // cyclic shift left

            Assert.AreEqual(0b00000000_00000000_00000000_00000001u,
                BitUtils.CyclicShiftLeft(0b10000000_00000000_00000000_00000000, 1, 32));

            Assert.AreEqual(0b01000000_00000000_00000000_00000000u,
                BitUtils.CyclicShiftLeft(0b10000000_00000000_00000000_00000000, 31, 32));

            Assert.AreEqual(0b11111000_00000111_11111000_00000111u,
                BitUtils.CyclicShiftLeft(0b11111111_00000000_11111111_00000000, 3, 32));

            Assert.AreEqual(0b10101010_10101010_10101010_10101010u,
                BitUtils.CyclicShiftLeft(0b10101010_10101010_10101010_10101010, 4, 32));

            Assert.AreEqual(0b01010101_01010101_01010101_01010101u,
                BitUtils.CyclicShiftLeft(0b10101010_10101010_10101010_10101010, 3, 32));

            Assert.AreEqual(0b01010101_01010000_00000101_01010101u,
                BitUtils.CyclicShiftLeft(0b10101010_10101010_00000000_10101010, 3, 32));

            Assert.AreEqual(0b00000000_11111111_00000000_11111111_00000000_11111111_00000000_11111111ul,
                BitUtils.CyclicShiftLeft(0b11111111_00000000_11111111_00000000_11111111_00000000_11111111_00000000ul, 8));
        }

        [TestMethod]
        public void Permutation()
        {
            // immutable
            var basePermutation = new List<byte>();
            var number = 1ul;
            for (byte i = 0; i < maxBitAmount; ++i)
                basePermutation.Add(i);
            for (int i = 0; i < maxBitAmount; ++i)
            {
                number = 1ul << i;
                Assert.AreEqual(number, BitUtils.Permutation(number, basePermutation));
            }

            number = 0b10101110u;
            Assert.AreEqual(number, BitUtils.Permutation(number, new List<byte>() { 0, 1, 2, 3, 4, 5, 6, 7 }));
            Assert.AreEqual(number, BitUtils.Permutation(number, new List<byte>() { 4, 2, 1, 5, 0, 7, 6, 3 }));

            // mutable
            number = 0b10101110u;
            Assert.AreEqual(0b10001111u, BitUtils.Permutation(number, new List<byte>() { 5, 3, 7, 1, 4, 0, 6, 2 }));

            number = 0b11111111_00000000_11111111_00000000u;
            Assert.AreEqual(0b01010011_11110000_10001111_01000110u,
                BitUtils.Permutation(number, new List<byte>() { 5, 31, 24, 1, 17, 16, 25, 0,
                    10, 28, 15, 14, 20, 4, 23, 9,
                    18, 6, 2, 7, 8, 27, 11, 12,
                    30, 13, 3, 19, 26, 21, 29, 22 }));
        }
    }
}