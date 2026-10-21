using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodingBackProp
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private async void runButton_Click(object sender, EventArgs e)
    {
      runButton.Enabled = false;
      outputBox.Clear();
      TextWriter oldOut = Console.Out;
      Console.SetOut(new TextBoxWriter(outputBox));
      try
      {
        await Task.Run(() => BackPropProgram.RunDemo());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error");
      }
      finally
      {
        Console.SetOut(oldOut);
        runButton.Enabled = true;
      }
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
      outputBox.Clear();
    }
    }

    public class TextBoxWriter : TextWriter
  {
    private TextBox box;

    public TextBoxWriter(TextBox box)
    {
      this.box = box;
    }

    public override Encoding Encoding
    {
      get { return Encoding.UTF8; }
    }

    public override void Write(char value)
    {
      Write(value.ToString());
    }

    public override void Write(char[] buffer, int index, int count)
    {
      Write(new string(buffer, index, count));
    }

    public override void Write(string value)
    {
      if (string.IsNullOrEmpty(value)) return;
      string text = value.Replace("\r\n", "\n").Replace("\n", "\r\n");
      if (box.IsDisposed) return;
      if (box.InvokeRequired)
        box.BeginInvoke(new Action(() => box.AppendText(text)));
      else
        box.AppendText(text);
    }
  }
}
