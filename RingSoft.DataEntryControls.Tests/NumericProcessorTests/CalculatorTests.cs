using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Tests.NumericProcessorTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Calculator_SimpleAddition()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.InitializeValue(null);
            
            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);
            
            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.TapeText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);
            processor.ProcessChar('2');
            Assert.AreEqual("32", control.EntryText);
            processor.ProcessChar('1');
            Assert.AreEqual("321", control.EntryText);
            processor.ProcessChar('0');
            Assert.AreEqual("3,210", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3,210 =", control.TapeText);
            Assert.AreEqual("3,215", control.EntryText);
            Assert.AreEqual(3215, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_MultipleAddition()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.InitializeValue(null);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.TapeText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.TapeText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);

            processor.ProcessChar('=');
            Assert.AreEqual(" 8 + 3 =", control.TapeText);
            Assert.AreEqual("11", control.EntryText);
            Assert.AreEqual(11, processor.ComittedValue);

            processor.ProcessCeButton();
            Assert.AreEqual("", control.TapeText);
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(11, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.TapeText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_Reset()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.InitializeValue(null);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.TapeText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 + 3 +", control.TapeText);
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('8');
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 + 8 =", control.TapeText);
            Assert.AreEqual("16", control.EntryText);
            Assert.AreEqual(16, processor.ComittedValue);

            processor.ProcessCeButton();
            Assert.AreEqual("", control.TapeText);
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(16, processor.ComittedValue);

            processor.ProcessChar('=');
            Assert.AreEqual(" 0 + 8 =", control.TapeText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_Initialize()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.InitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.TapeText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.TapeText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_OperatorButtonRightAfterEqualsButton()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.InitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessCButton();
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(0, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.TapeText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.TapeText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);

            processor.ProcessChar('+');
            Assert.AreEqual(" 8 +", control.TapeText);
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 8 +", control.TapeText);
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('1');
            Assert.AreEqual(" 8 +", control.TapeText);
            Assert.AreEqual("1", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 8 + 1 +", control.TapeText);
            Assert.AreEqual("9", control.EntryText);
        }

        [TestMethod]
        public void Calculator_AddToInitialValue()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.InitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessChar('+');
            Assert.AreEqual(" 100 +", control.TapeText);
            Assert.AreEqual("100", control.EntryText);

            processor.ProcessChar('1');
            Assert.AreEqual(" 100 +", control.TapeText);
            Assert.AreEqual("1", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 100 + 1 =", control.TapeText);
            Assert.AreEqual("101", control.EntryText);
            Assert.AreEqual(101, processor.ComittedValue);
        }
    }
}
