using System.Collections.Generic;
using NUnit.Framework;

namespace EditModeTests
{
    public class DictionaryInspectionTest
    {

        class HashTest
        {
            public int Value { get; set; }
            public override int GetHashCode()
            {
                return Value;
            }
        }

        struct hashTest2
        {
            public int Value { get; set; }
            public override int GetHashCode()
            {
                return Value;
            }
        }

        class EqualsTest
        {
            public int Value { get; set; }
            public static bool operator ==(EqualsTest a, EqualsTest b)
            {
                return a.Value == b.Value;
            }
            public static bool operator !=(EqualsTest a, EqualsTest b)
            {
                return a.Value != b.Value;
            }
        }

        struct EqualsTest2
        {
            public int Value { get; set; }
            public static bool operator ==(EqualsTest2 a, EqualsTest2 b)
            {
                return a.Value == b.Value;
            }
            public static bool operator !=(EqualsTest2 a, EqualsTest2 b)
            {
                return a.Value != b.Value;
            }
        }
        struct EqualsTest3
        {
            public int Value { get; set; }
            public override bool Equals(object obj)
            {
                return Value == ((EqualsTest3)obj).Value;
            }
        }

        [Test]
        public void Dictionary_Keyが参照型の場合の等価の比較にはHashCodeは使われない()
        {

            var dict = new Dictionary<HashTest, string>();
            var hashTest1 = new HashTest() { Value = 1 };
            var hashTest2 = new HashTest() { Value = 1 };
            dict[hashTest1] = "1";
            dict[hashTest2] = "2";
            Assert.That(dict.Count, Is.EqualTo(2));
        }

        [Test]
        public void Dictionary_Keyが参照型場合の等価の比較には等価演算子を使わない()
        {
            var dict = new Dictionary<EqualsTest, string>();
            var EqualsTest1 = new EqualsTest() { Value = 1 };
            var EqualsTest2 = new EqualsTest() { Value = 1 };
            dict[EqualsTest1] = "1";
            dict[EqualsTest2] = "2";
            Assert.That(EqualsTest1 == EqualsTest2, Is.True);
            Assert.That(dict.Count, Is.EqualTo(2));
        }

        [Test]
        public void Dictonary_Keyが参照型の場合は等価の比較には参照を使う()
        {
            var dict = new Dictionary<object, string>();
            var obj1 = new object();
            var obj2 = obj1;
            dict[obj1] = "1";
            dict[obj2] = "2";
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void Dictionary_Keyが値型の場合は等価の比較には値を使う()
        {
            var dict = new Dictionary<int, string>();
            var int1 = 1;
            var int2 = 1;
            dict[int1] = "1";
            dict[int2] = "2";
            Assert.That(dict.Count, Is.EqualTo(1));
        }
        [Test]
        public void Dictionary_Keyが値型の場合にはKeyの等価の比較には等価演算子を使う()
        {
            var dict = new Dictionary<EqualsTest2, string>();
            var EqualsTest1 = new EqualsTest2() { Value = 1 };
            var EqualsTest2 = new EqualsTest2() { Value = 1 };
            dict[EqualsTest1] = "1";
            dict[EqualsTest2] = "2";
            Assert.That(EqualsTest1 == EqualsTest2, Is.True);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void Dictionary_Keyが値型の場合にはKeyの等価の比較にはEqualsを使う()
        {
            var dict = new Dictionary<EqualsTest3, string>();
            var EqualsTest1 = new EqualsTest3() { Value = 1 };
            var EqualsTest2 = new EqualsTest3() { Value = 1 };
            dict[EqualsTest1] = "1";
            dict[EqualsTest2] = "2";
            Assert.That(EqualsTest1.Equals(EqualsTest2), Is.True);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void Dictionary_Keyが値型の場合にはKeyの等価の比較にはHasCodeが使われる()
        {
            var dict = new Dictionary<hashTest2, string>();
            var hashTest1 = new hashTest2() { Value = 1 };
            var hashTest2 = new hashTest2() { Value = 1 };
            dict[hashTest1] = "1";
            dict[hashTest2] = "2";
            Assert.That(dict.Count, Is.EqualTo(1));
        }

    }
}