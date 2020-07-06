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

            processor.ReinitializeValue(null);
            
            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);
            
            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.EquationText);
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
            Assert.AreEqual(" 5 + 3,210 =", control.EquationText);
            Assert.AreEqual("3,215", control.EntryText);
            Assert.AreEqual(3215, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_MultipleAddition()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.ReinitializeValue(null);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.EquationText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.EquationText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);

            processor.ProcessChar('=');
            Assert.AreEqual(" 8 + 3 =", control.EquationText);
            Assert.AreEqual("11", control.EntryText);
            Assert.AreEqual(11, processor.ComittedValue);

            processor.ProcessCeButton();
            Assert.AreEqual("", control.EquationText);
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(11, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.EquationText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_Reset()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.ReinitializeValue(null);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.EquationText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 + 3 +", control.EquationText);
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('8');
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 + 8 =", control.EquationText);
            Assert.AreEqual("16", control.EntryText);
            Assert.AreEqual(16, processor.ComittedValue);

            processor.ProcessCeButton();
            Assert.AreEqual("", control.EquationText);
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(16, processor.ComittedValue);

            processor.ProcessChar('=');
            Assert.AreEqual(" 0 + 8 =", control.EquationText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_Initialize()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.ReinitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.EquationText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.EquationText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_OperatorButtonRightAfterEqualsButton()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.ReinitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessCButton();
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(0, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.EquationText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.EquationText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);

            processor.ProcessChar('+');
            Assert.AreEqual(" 8 +", control.EquationText);
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 8 +", control.EquationText);
            Assert.AreEqual("8", control.EntryText);

            processor.ProcessChar('1');
            Assert.AreEqual(" 8 +", control.EquationText);
            Assert.AreEqual("1", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 8 + 1 +", control.EquationText);
            Assert.AreEqual("9", control.EntryText);
        }

        [TestMethod]
        public void Calculator_OperatorButtonRightAfterEqualsButton2()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.ReinitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessCButton();
            Assert.AreEqual("0", control.EntryText);
            Assert.AreEqual(0, processor.ComittedValue);

            processor.ProcessChar('5');
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 5 +", control.EquationText);
            Assert.AreEqual("5", control.EntryText);

            processor.ProcessChar('3');
            Assert.AreEqual("3", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 5 + 3 =", control.EquationText);
            Assert.AreEqual("8", control.EntryText);
            Assert.AreEqual(8, processor.ComittedValue);

            processor.ProcessChar('2');
            Assert.AreEqual("2", control.EntryText);

            processor.ProcessChar('+');
            Assert.AreEqual(" 2 +", control.EquationText);
            Assert.AreEqual("2", control.EntryText);
        }

        [TestMethod]
        public void Calculator_AddToInitialValue()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);

            processor.ReinitializeValue(100);
            Assert.AreEqual("100", control.EntryText);
            Assert.AreEqual(100, processor.ComittedValue);

            processor.ProcessChar('+');
            Assert.AreEqual(" 100 +", control.EquationText);
            Assert.AreEqual("100", control.EntryText);

            processor.ProcessChar('1');
            Assert.AreEqual(" 100 +", control.EquationText);
            Assert.AreEqual("1", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 100 + 1 =", control.EquationText);
            Assert.AreEqual("101", control.EntryText);
            Assert.AreEqual(101, processor.ComittedValue);
        }

        [TestMethod]
        public void Calculator_PlusMinus()
        {
            var control = new TestCalculatorControl();
            var processor = new CalculatorProcessor(control);


            processor.ProcessChar('4');
            processor.ProcessChar('5');
            processor.ProcessChar('5');
            processor.ProcessChar('+');
            Assert.AreEqual(" 455 +", control.EquationText);
            Assert.AreEqual("455", control.EntryText);

            processor.ProcessChar('1');
            processor.ProcessPlusMinusButton();
            Assert.AreEqual("-1", control.EntryText);

            processor.ProcessChar('=');
            Assert.AreEqual(" 455 + -1 =", control.EquationText);
            Assert.AreEqual("454", control.EntryText);
            Assert.AreEqual(454, processor.ComittedValue);
        }
    }
}
