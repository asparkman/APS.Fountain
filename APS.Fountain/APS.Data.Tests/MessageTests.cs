using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    public abstract class MessageTests<TTypeClass, TTypeEnum, TFieldEnum>
        where TTypeClass : Message<TTypeEnum, TFieldEnum>, new()
        where TTypeEnum : struct, IConvertible
        where TFieldEnum : struct, IConvertible
    {
        public static readonly byte[] BYTES = new byte[] { 1, 0, 254, 255 };

        public abstract IEnumerable<TTypeClass> IterateThroughCommonChanges();
        public abstract IEnumerable<TFieldEnum> CommonFieldThatChanged();

        public abstract IEnumerable<TTypeClass> IterareThroughChanges(TTypeClass start);
        public abstract IEnumerable<TFieldEnum> FieldThatChanged();

        [Test]
        public void MessageType_MatchesClassName()
        {
            var typeClassName = typeof(TTypeClass).Name.ToUpper();
            var messageType = new TTypeClass().Type;
            var typeEnumName = Enum.GetName(typeof(TTypeEnum), messageType).Replace("_", "");
            Assert.AreEqual(typeClassName, typeEnumName);
        }

        [Test]
        public void AssertCorrectField()
        {
            var allFieldsThatChanged = CommonFieldThatChanged().ToList();
            var secondFieldsThatChanged = FieldThatChanged().ToList();
            allFieldsThatChanged.AddRange(secondFieldsThatChanged);
            var allChanges = IterateThroughCommonChanges().Select(x => (TTypeClass)x.Clone()).ToList();
            var secondChanges = IterareThroughChanges(allChanges.Last()).Select(x => (TTypeClass)x.Clone()).ToList();
            allChanges.AddRange(secondChanges);

            Assert.AreEqual(allFieldsThatChanged.Count, allChanges.Count);

            TTypeClass lastChanged = default(TTypeClass);
            TFieldEnum fieldThatChanged = default(TFieldEnum);
            // Go through all the changes, and make sure the byte index was changed correctly.
            for(int i = 0; i < allChanges.Count; i++)
            {
                var currentChange = allChanges[i];
                fieldThatChanged = allFieldsThatChanged[i];

                if(lastChanged != null)
                {
                    Assert.AreEqual(currentChange.Bytes.Length, lastChanged.Bytes.Length);
                    for(int j = 0; j < currentChange.Bytes.Length; j++)
                    {
                        if(fieldThatChanged.ToInt32(CultureInfo.CurrentCulture) != j)
                            Assert.AreEqual(lastChanged.Bytes[j], currentChange.Bytes[j]);
                        else
                            Assert.AreNotEqual(lastChanged.Bytes[j], currentChange.Bytes[j]);
                    }
                }
                

                // Update last field that changed as last thing before next iteration.
                lastChanged = allChanges[i];
            }

        }
    }
}
