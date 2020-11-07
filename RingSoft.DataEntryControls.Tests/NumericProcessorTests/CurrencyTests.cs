using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Tests.NumericProcessorTests
{
    [TestClass]
    public class CurrencyTests
    {
        private static DecimalEditControlSetup _setup;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _setup = new DecimalEditControlSetup()
            {
                DataEntryMode = DataEntryModes.FormatOnEntry,
                FormatType = DecimalEditFormatTypes.Currency,
                Precision = 2,
                CultureId = "pt-BR"
            };
        }

        [TestMethod]
        public void CurrencyNumber_StraightDataEntry()
        {
            var control = new TestNumericControl();
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '1');
            Assert.AreEqual("R$ 1", control.Text, "First Digit");
            Assert.AreEqual(4, control.SelectionStart, "First Digit");
            Assert.AreEqual(1, processor.Value, "First Digit Value");

            processor.ProcessChar(_setup, '2');
            Assert.AreEqual("R$ 12", control.Text, "Second Digit");
            Assert.AreEqual(5, control.SelectionStart, "Second Digit");
            Assert.AreEqual(12, processor.Value, "Second Digit Value");

            processor.ProcessChar(_setup, '3');
            Assert.AreEqual("R$ 123", control.Text, "Third Digit");
            Assert.AreEqual(6, control.SelectionStart, "Third Digit");
            Assert.AreEqual(123, processor.Value, "Third Digit Value");

            processor.ProcessChar(_setup, '4');
            Assert.AreEqual("R$ 1.234", control.Text, "Fourth Digit");
            Assert.AreEqual(8, control.SelectionStart, "Fourth Digit");
            Assert.AreEqual(1234, processor.Value, "Fourth Digit Value");

            processor.ProcessChar(_setup, '.');
            Assert.AreEqual("R$ 1.234,", control.Text, "Decimal Point");
            Assert.AreEqual(9, control.SelectionStart, "Decimal Point");
            Assert.AreEqual(1234, processor.Value, "Decimal Point Value");

            processor.ProcessChar(_setup, '5');
            Assert.AreEqual("R$ 1.234,5", control.Text, "First Decimal Digit");
            Assert.AreEqual(10, control.SelectionStart, "First Decimal Digit");
            Assert.AreEqual((decimal)1234.5, processor.Value, "First Decimal Digit Value");

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("R$ 1.234,56", control.Text, "Second Decimal Digit");
            Assert.AreEqual(11, control.SelectionStart, "Second Decimal Digit");
            Assert.AreEqual((decimal)1234.56, processor.Value, "Second Decimal Digit Value");
        }

        [TestMethod]
        public void CurrencyNumber_MiddleReplace()
        {
            var control = new TestNumericControl()
            {
                Text = "R$ 1.234.567,89",
                SelectionStart = 5,
                SelectionLength = 5
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("R$ 1.667,89", control.Text, "Middle Replace");
            Assert.AreEqual(6, control.SelectionStart, "Middle Replace");
            Assert.AreEqual((decimal)1667.89, processor.Value, "Middle Replace Value");

            processor.ProcessChar(_setup, '7');
            Assert.AreEqual("R$ 16.767,89", control.Text, "Middle Replace Add First Digit");
            Assert.AreEqual(7, control.SelectionStart, "Middle Replace Add First Digit");
            Assert.AreEqual((decimal)16767.89, processor.Value, "Middle Replace Add First Digit Value");

            processor.ProcessChar(_setup, '8');
            Assert.AreEqual("R$ 167.867,89", control.Text, "Middle Replace Add Second Digit");
            Assert.AreEqual(8, control.SelectionStart, "Middle Replace Add Second Digit");
            Assert.AreEqual((decimal)167867.89, processor.Value, "Middle Replace Add Second Digit Value");

            processor.ProcessChar(_setup, '9');
            Assert.AreEqual("R$ 1.678.967,89", control.Text, "Middle Replace Add Third Digit");
            Assert.AreEqual(10, control.SelectionStart, "Middle Replace Add Third Digit");
            Assert.AreEqual((decimal)1678967.89, processor.Value, "Middle Replace Add Third Digit Value");
        }

        [TestMethod]
        public void CurrencyNumber_MiddleReplace1()
        {
            var control = new TestNumericControl()
            {
                Text = "R$ 1.234.567,89",
                SelectionStart = 5,
                SelectionLength = 3
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("R$ 16.567,89", control.Text, "Middle Replace1");
            Assert.AreEqual(6, control.SelectionStart, "Middle Replace1");
            Assert.AreEqual((decimal)16567.89, processor.Value, "Middle Replace1 Value");

            processor.ProcessChar(_setup, '7');
            Assert.AreEqual("R$ 167.567,89", control.Text, "Middle Replace1 Add First Digit");
            Assert.AreEqual(7, control.SelectionStart, "Middle Replace1 Add First Digit");
            Assert.AreEqual((decimal)167567.89, processor.Value, "Middle Replace1 Add First Digit Value");

            processor.ProcessChar(_setup, '8');
            Assert.AreEqual("R$ 1.678.567,89", control.Text, "Middle Replace1 Add Second Digit");
            Assert.AreEqual(9, control.SelectionStart, "Middle Replace1 Add Second Digit");
            Assert.AreEqual((decimal)1678567.89, processor.Value, "Middle Replace1 Add Second Digit Value");
        }

        [TestMethod]
        public void CurrencyNumber_MiddleReplace2()
        {
            var control = new TestNumericControl()
            {
                Text = "R$ 1.234.567,89",
                SelectionStart = 5,
                SelectionLength = 4
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("R$ 16.567,89", control.Text, "Middle Replace2");
            Assert.AreEqual(6, control.SelectionStart, "Middle Replace2");
            Assert.AreEqual((decimal)16567.89, processor.Value, "Middle Replace2 Value");
        }

        [TestMethod]
        public void CurrencyNumber_ValidateDecimal()
        {
            var control = new TestNumericControl()
            {
                Text = "R$ 1.234.567,89",
                SelectionStart = 13,
                SelectionLength = 0
            };
            var processor = new DataEntryNumericControlProcessor(control);

            var result = processor.ProcessChar(_setup, '.');
            Assert.AreEqual(ProcessCharResults.ValidationFailed, result, "Validate Decimal Add Second Decimal Point");

            result = processor.ProcessChar(_setup, '7');
            Assert.AreEqual(ProcessCharResults.ValidationFailed, result, "Validate Decimal Add Third Decimal Digit");
        }

        [TestMethod]
        public void CurrencyNumber_AddDigitBeforeCurrencySymbol()
        {
            var control = new TestNumericControl()
            {
                Text = "R$ 1.234.567,89",
                SelectionStart = 1,
                SelectionLength = 0
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(_setup, '1');
            Assert.AreEqual("R$ 11.234.567,89", control.Text, "Add Digit Before Currency Symbol");
            Assert.AreEqual(4, control.SelectionStart, "Add Digit Before Currency Symbol");
            Assert.AreEqual((decimal)11234567.89, processor.Value, "Add Digit Before Currency Symbol");

            control.SelectionStart = 1;
            control.SelectionLength = 4;
            processor.ProcessChar(_setup, '6');
            Assert.AreEqual("R$ 6.234.567,89", control.Text, "Add Digit Before Currency Symbol");
            Assert.AreEqual(4, control.SelectionStart, "Add Digit Before Currency Symbol");
            Assert.AreEqual((decimal)6234567.89, processor.Value, "Add Digit Before Currency Symbol");
        }

        [TestMethod]
        public void CurrencyNumber_AddDigitAfterSuffixCurrencySymbol()
        {
            var setup = new DecimalEditControlSetup()
            {
                DataEntryMode = DataEntryModes.FormatOnEntry,
                FormatType = DecimalEditFormatTypes.Currency,
                Precision = 2,
                CultureId = "sv-SE"
            };

            var control = new TestNumericControl()
            {
                Text = "1 234 567,8 kr",
                SelectionStart = 13,
                SelectionLength = 0
            };
            var processor = new DataEntryNumericControlProcessor(control);

            processor.ProcessChar(setup, '9');
            Assert.AreEqual("1 234 567,89 kr", control.Text, "Add Digit After Suffix Currency Symbol");
            Assert.AreEqual(12, control.SelectionStart, "Add Digit After Suffix Currency Symbol");
            Assert.AreEqual((decimal)1234567.89, processor.Value, "Add Digit After Suffix Currency Symbol");

            control.SelectionStart = 9;
            control.SelectionLength = 5;
            processor.ProcessChar(setup, '6');
            Assert.AreEqual("12 345 676 kr", control.Text, "Add Digit After Suffix Currency Symbol");
            Assert.AreEqual(10, control.SelectionStart, "Add Digit After Suffix Currency Symbol");
            Assert.AreEqual(12345676, processor.Value, "Add Digit After Suffix Currency Symbol");
        }
    }
}
