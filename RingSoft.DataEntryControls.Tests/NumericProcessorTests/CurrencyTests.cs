using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Tests.NumericProcessorTests
{
    [TestClass]
    public class CurrencyTests
    {
        private static DataEntryNumericEditSetup _setup;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _setup = new DataEntryNumericEditSetup()
            {
                DataEntryMode = DataEntryModes.FormatOnEntry,
                EditFormatType = NumericEditFormatTypes.Currency,
                Precision = 2
            };
        }

        [TestMethod]
        public void CurrencyNumber_StraightDataEntry()
        {
            var control = new TestNumericControl();
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '1');
            Assert.AreEqual("$1", control.Text, "First Digit");
            Assert.AreEqual(2, control.SelectionStart, "First Digit");
            Assert.AreEqual(1, (double)processor.Value, "First Digit Value");

            processor.ProcessChar(_setup, '2');
            Assert.AreEqual("$12", control.Text, "Second Digit");
            Assert.AreEqual(3, control.SelectionStart, "Second Digit");
            Assert.AreEqual(12, (double)processor.Value, "Second Digit Value");

            processor.ProcessChar(_setup, '3');
            Assert.AreEqual("$123", control.Text, "Third Digit");
            Assert.AreEqual(4, control.SelectionStart, "Third Digit");
            Assert.AreEqual(123, (double)processor.Value, "Third Digit Value");

            processor.ProcessChar(_setup, '4');
            Assert.AreEqual("$1,234", control.Text, "Fourth Digit");
            Assert.AreEqual(6, control.SelectionStart, "Fourth Digit");
            Assert.AreEqual(1234, (double)processor.Value, "Fourth Digit Value");

            processor.ProcessDecimal(_setup);
            Assert.AreEqual("$1,234.", control.Text, "Decimal Point");
            Assert.AreEqual(7, control.SelectionStart, "Decimal Point");
            Assert.AreEqual(1234, (double)processor.Value, "Decimal Point Value");

            processor.ProcessChar(_setup, '5');
            Assert.AreEqual("$1,234.5", control.Text, "First Decimal Digit");
            Assert.AreEqual(8, control.SelectionStart, "First Decimal Digit");
            Assert.AreEqual(1234.5, (double)processor.Value, "First Decimal Digit Value");

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("$1,234.56", control.Text, "Second Decimal Digit");
            Assert.AreEqual(9, control.SelectionStart, "Second Decimal Digit");
            Assert.AreEqual(1234.56, (double)processor.Value, "Second Decimal Digit Value");
        }

        [TestMethod]
        public void CurrencyNumber_MiddleReplace()
        {
            var control = new TestNumericControl()
            {
                Text = "$1,234,567.89",
                SelectionStart = 3,
                SelectionLength = 5
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("$1,667.89", control.Text, "Middle Replace");
            Assert.AreEqual(4, control.SelectionStart, "Middle Replace");
            Assert.AreEqual(1667.89, (double) processor.Value, "Middle Replace Value");

            processor.ProcessChar(_setup, '7');
            Assert.AreEqual("$16,767.89", control.Text, "Middle Replace Add First Digit");
            Assert.AreEqual(5, control.SelectionStart, "Middle Replace Add First Digit");
            Assert.AreEqual(16767.89, (double)processor.Value, "Middle Replace Add First Digit Value");

            processor.ProcessChar(_setup, '8');
            Assert.AreEqual("$167,867.89", control.Text, "Middle Replace Add Second Digit");
            Assert.AreEqual(6, control.SelectionStart, "Middle Replace Add Second Digit");
            Assert.AreEqual(167867.89, (double)processor.Value, "Middle Replace Add Second Digit Value");

            processor.ProcessChar(_setup, '9');
            Assert.AreEqual("$1,678,967.89", control.Text, "Middle Replace Add Third Digit");
            Assert.AreEqual(8, control.SelectionStart, "Middle Replace Add Third Digit");
            Assert.AreEqual(1678967.89, (double)processor.Value, "Middle Replace Add Third Digit Value");
        }

        [TestMethod]
        public void CurrencyNumber_MiddleReplace1()
        {
            var control = new TestNumericControl()
            {
                Text = "$1,234,567.89",
                SelectionStart = 3,
                SelectionLength = 3
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("$16,567.89", control.Text, "Middle Replace1");
            Assert.AreEqual(4, control.SelectionStart, "Middle Replace1");
            Assert.AreEqual(16567.89, (double)processor.Value, "Middle Replace1 Value");

            processor.ProcessChar(_setup, '7');
            Assert.AreEqual("$167,567.89", control.Text, "Middle Replace1 Add First Digit");
            Assert.AreEqual(5, control.SelectionStart, "Middle Replace1 Add First Digit");
            Assert.AreEqual(167567.89, (double)processor.Value, "Middle Replace1 Add First Digit Value");

            processor.ProcessChar(_setup, '8');
            Assert.AreEqual("$1,678,567.89", control.Text, "Middle Replace1 Add Second Digit");
            Assert.AreEqual(7, control.SelectionStart, "Middle Replace1 Add Second Digit");
            Assert.AreEqual(1678567.89, (double)processor.Value, "Middle Replace1 Add Second Digit Value");
        }

        [TestMethod]
        public void CurrencyNumber_MiddleReplace2()
        {
            var control = new TestNumericControl()
            {
                Text = "$1,234,567.89",
                SelectionStart = 3,
                SelectionLength = 4
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("$16,567.89", control.Text, "Middle Replace2");
            Assert.AreEqual(4, control.SelectionStart, "Middle Replace2");
            Assert.AreEqual(16567.89, (double) processor.Value, "Middle Replace2 Value");
        }

        [TestMethod]
        public void CurrencyNumber_ValidateDecimal()
        {
            var control = new TestNumericControl()
            {
                Text = "$1,234,567.89",
                SelectionStart = 11,
                SelectionLength = 0
            };
            var processor = new DataEntryNumericControlProcessor(control);

            var result = processor.ProcessDecimal(_setup);
            Assert.AreEqual(ProcessCharResults.ValidationFailed, result, "Validate Decimal Add Second Decimal Point");

            result = processor.ProcessChar(_setup, '7');
            Assert.AreEqual(ProcessCharResults.ValidationFailed, result, "Validate Decimal Add Third Decimal Digit");
        }

        //[TestMethod]
        //public void CurrencyNumber_AddDigitBeforeCurrencySymbol()
        //{
        //    var control = new TestNumericControl()
        //    {
        //        Text = "$1,234,567.89",
        //        SelectionStart = 0,
        //        SelectionLength = 0
        //    };
        //    var processor = new DataEntryNumericControlProcessor(control);

        //    processor.ProcessChar(_setup, '1');
        //    Assert.AreEqual("$11,234,567.89", control.Text, "Add Digit Before Currency Symbol");
        //    Assert.AreEqual(2, control.SelectionStart, "Add Digit Before Currency Symbol");
        //    Assert.AreEqual(11234567.89, (double)processor.Value, "Add Digit Before Currency Symbol");
        //}
    }
}
