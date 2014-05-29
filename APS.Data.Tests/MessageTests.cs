using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    /// <summary>
    /// Holds common tests and testing functionality for all 
    /// <c>Message&lt;TTypeEnum, TFieldEnum&gt;</c> classes.
    /// </summary>
    /// <typeparam name="TTypeClass">The message class to be tested.</typeparam>
    /// <typeparam name="TTypeEnum">The message class's enumeration of types.</typeparam>
    /// <typeparam name="TFieldEnum">The message class's enumeration of fields.</typeparam>
    public abstract class MessageTests<TTypeClass, TTypeEnum, TFieldEnum>
        where TTypeClass : Message<TTypeEnum, TFieldEnum>, new()
        where TTypeEnum : struct, IConvertible
        where TFieldEnum : struct, IConvertible
    {
        /// <summary>
        /// Common bytes used to help generate the 
        /// <c>IterateThroughCommonChanges</c>, and 
        /// <c>IterareThroughChanges</c> enumerables.
        /// </summary>
        public static readonly byte[] BYTES = new byte[] { 1, 0, 254, 255 };

        /// <summary>
        /// Iterates though changes common to all <c>Message</c> types.  Each 
        /// new object returned is different by one field from the last.
        /// </summary>
        /// <returns>A new object that is exactly the same as the last one, 
        /// except for one byte is different.</returns>
        public abstract IEnumerable<TTypeClass> IterateThroughCommonChanges();
        /// <summary>
        /// Iterates through each field that was changed by 
        /// <c>IterateThroughCommonChanges</c>.
        /// </summary>
        /// <returns>The field that is different from the last one for the 
        /// object returned by <c>IterateThroughCommonChanges</c>.</returns>
        public abstract IEnumerable<TFieldEnum> CommonFieldThatChanged();

        /// <summary>
        /// Iterates though changes specific to all this TTypeClass.  Each new 
        /// object returned is different by one field from the last.
        /// </summary>
        /// <returns>A new object that is exactly the same as the last one, 
        /// except for one byte is different.</returns>
        public abstract IEnumerable<TTypeClass> IterareThroughChanges(TTypeClass start);
        /// <summary>
        /// Iterates through each field that was changed by 
        /// <c>IterareThroughChanges</c>.
        /// </summary>
        /// <returns>The field that is different from the last one for the 
        /// object returned by <c>IterareThroughChanges</c>.</returns>
        public abstract IEnumerable<TFieldEnum> FieldThatChanged();

        /// <summary>
        /// Makes sure that the <c>Type</c> property of the <c>Message</c> 
        /// class matches its class name.
        /// </summary>
        [Test]
        public void MessageType_MatchesClassName()
        {
            var typeClassName = typeof(TTypeClass).Name.ToUpper();
            var messageType = new TTypeClass().Type;
            var typeEnumName = Enum.GetName(typeof(TTypeEnum), messageType).Replace("_", "");
            Assert.AreEqual(typeClassName, typeEnumName);
        }

        /// <summary>
        /// Makes sure that ever <c>Message</c> correctly stores each property 
        /// according to its message layout.
        /// 
        /// It does this by using the overriden values returned by the 
        /// following methods.
        /// <c>IterateThroughCommonChanges</c>
        /// <c>CommonFieldThatChanged</c>
        /// <c>IterareThroughChanges(TTypeClass)</c>
        /// <c>FieldThatChanged</c>
        /// </summary>
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
