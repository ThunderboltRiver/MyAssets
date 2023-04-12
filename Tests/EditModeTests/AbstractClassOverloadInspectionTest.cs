using System.Collections.Generic;
using NUnit.Framework;

namespace EditModeTests
{
    public class AbstractClassOverloadInspectionTest
    {
        private abstract class AbstractClass
        {
            public static bool operator ==(AbstractClass a, AbstractClass b)
            {
                return a.GetType() == b.GetType();
            }
            public static bool operator !=(AbstractClass a, AbstractClass b)
            {
                return a.GetType() != b.GetType();
            }
            public override bool Equals(object obj)
            {
                return this.GetType() == obj.GetType();
            }

            public override int GetHashCode()
            {
                return this.GetType().GetHashCode();
            }

        }

        class Class1 : AbstractClass { }

        [Test]
        public void 同じ具象クラスでは同じにできる()
        {
            var a = new Class1();
            var b = new Class1();
            Assert.That(a == b, Is.True);
        }
        [Test]
        public void Dictionary_同じ具象クラスのKeyではKeyとして同じにできる()
        {
            var a = new Class1();
            var b = new Class1();
            var dic = new Dictionary<AbstractClass, int>
            {
                { a, 1 }
            };
            Assert.That(dic.ContainsKey(b), Is.True);
        }
    }
}