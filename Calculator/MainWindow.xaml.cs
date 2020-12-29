using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		MathEvaluator mathEvaluator = new MathEvaluator();

		public MainWindow() {

			InitializeComponent();
			
		}
		private void ReEvaluate() {
			if (InputBase.Text == "") {
				mathEvaluator.numBase = 10;
			}
			else {
				mathEvaluator.numBase = Int32.Parse(InputBase.Text);
			}

			if (MathInput.Text == "") {
				MathOutput.Text = "0";
			}
			else {
				try {
					
					checked {
						Fraction result = mathEvaluator.calculate(MathInput.Text);
						if ((bool)CharMode.IsChecked) {
							result.Simplify();
							if (result.denomonator == 1 && result.numerator < 0xFF) {
								char c = (char)result.numerator;
								//control characters
								if (c < ' ') {  //control characters
									switch (c) {
										case (char)0:
											MathOutput.Text = "Null";
											break;
										case (char)1:
											MathOutput.Text = "StartOfHeading";
											break;
										case (char)2:
											MathOutput.Text = "StartOfText";
											break;
										case (char)3:
											MathOutput.Text = "EndOfText";
											break;
										case (char)4:
											MathOutput.Text = "EndOfTransmission";
											break;
										case (char)5:
											MathOutput.Text = "Enquiry";
											break;
										case (char)6:
											MathOutput.Text = "Acknowledge";
											break;
										case (char)7:
											MathOutput.Text = "Bell";
											break;
										case (char)8:
											MathOutput.Text = "Backspace";
											break;
										case (char)9:
											MathOutput.Text = "HorizontalTab";
											break;
										case (char)10:
											MathOutput.Text = "LineFeed";
											break;
										case (char)11:
											MathOutput.Text = "VerticalTab";
											break;
										case (char)12:
											MathOutput.Text = "FormFeed";
											break;
										case (char)13:
											MathOutput.Text = "CarriageReturn";
											break;
										case (char)14:
											MathOutput.Text = "ShiftOut";
											break;
										case (char)15:
											MathOutput.Text = "ShiftIn";
											break;
										case (char)16:
											MathOutput.Text = "DataLinkEscape";
											break;
										case (char)17:
											MathOutput.Text = "DeviceControl1";
											break;
										case (char)18:
											MathOutput.Text = "DeviceControl2";
											break;
										case (char)19:
											MathOutput.Text = "DeviceControl3";
											break;
										case (char)20:
											MathOutput.Text = "DeviceControl4";
											break;
										case (char)21:
											MathOutput.Text = "NagativeAcknowledge";
											break;
										case (char)22:
											MathOutput.Text = "SynchronousIdle";
											break;
										case (char)23:
											MathOutput.Text = "EndOfTrasmissionBlock";
											break;
										case (char)24:
											MathOutput.Text = "Cancel";
											break;
										case (char)25:
											MathOutput.Text = "EndOfMedium";
											break;
										case (char)26:
											MathOutput.Text = "Substitute";
											break;
										case (char)27:
											MathOutput.Text = "Escape";
											break;
										case (char)28:
											MathOutput.Text = "FileSeperator";
											break;
										case (char)29:
											MathOutput.Text = "GroupSeperator";
											break;
										case (char)30:
											MathOutput.Text = "RecordSeperator";
											break;
										case (char)31:
											MathOutput.Text = "UnitSeperator";
											break;
									}


								}

								//printable ascii
								else if (c == ' ') {
									MathOutput.Text = "space";
								}
								else {
									MathOutput.Text = new string(c, 1);
								}
									
								
								
							}
						}
						else if ((bool)FractionCheckBox.IsChecked) {
							result.Simplify();
							MathOutput.Text = result.AsFraction();
						}
						else {
							MathOutput.Text = result.AsDecimalString();
						}
					}
					MathOutput.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));


				} catch (Exception ex) {
					MathOutput.Foreground = new SolidColorBrush(Color.FromRgb(200, 10, 10));
					Console.WriteLine(ex.ToString());
				}
			}
		}

		private void MathInput_Changed(object sender, TextChangedEventArgs e) {
			using (MathInput.DeclareChangeBlock()) {
				foreach (var c in e.Changes) {
					if (c.AddedLength == 0) continue;
					MathInput.Select(c.Offset, c.AddedLength);
					if (MathInput.SelectedText.Contains('p')) {
						MathInput.SelectedText = MathInput.SelectedText.Replace('p', 'π');
					}
					MathInput.Select(c.Offset + c.AddedLength, 0);
				}
			}
			ReEvaluate();
		}
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
			ReEvaluate();
		}

		private void CheckBox_Click(object sender, RoutedEventArgs e) {
			ReEvaluate();
		}

		private void OutputBase_CheckTextInput(object sender, TextCompositionEventArgs e) {
			char c = e.Text[0];
			e.Handled = !(Char.IsDigit(c) &&
						 (OutputBase.Text == "" ||
						 OutputBase.Text == "1" &&
						 (c - '0') >= 0 &&
						 (c - '0') <= 6));
		}

		private void InputBase_CheckTextInput(object sender, TextCompositionEventArgs e) {
			char c = e.Text[0];
			e.Handled = !(Char.IsDigit(c) &&
						 (InputBase.Text == "" ||
						 InputBase.Text == "1" &&
						 (c - '0') >= 0 &&
						 (c - '0') <= 6));
		}

		
	}
}
