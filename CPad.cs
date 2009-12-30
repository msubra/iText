using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Data;

class CPad: System.Windows.Forms.Form //Creating a class inherited from the Form class
{
	MainMenu mnuMain;
	MenuItem mnuFile;
	MenuItem mnuEdit;
	MenuItem mnuFormat;
	MenuItem mnuHelp;

	RichTextBox txtCPad;
	StatusBar sbCPad;
	Timer tmrCPad;
	
	static string TextToFind="";
	static string TextToReplace="";

	static bool IsContentModified=false;
	static bool IsDirectionDownward=true; //true means "Down" false  means "Up"
	bool flag=false;

	public CPad()
	{
		txtCPad=new RichTextBox();
		sbCPad=new StatusBar();
			
		Statusbar();

		sbCPad.ShowPanels=true;
		txtCPad.Location=new Point(0,0);
		txtCPad.Size=new Size(300,275-sbCPad.Height);
		txtCPad.Anchor=AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
		txtCPad.Multiline=true;
		txtCPad.AcceptsTab=true;
		txtCPad.HideSelection=false;
		txtCPad.ScrollBars=RichTextBoxScrollBars.Both;
		txtCPad.WordWrap=true;
		txtCPad.AutoSize=false;
		txtCPad.SelectionFont = new Font("Courier", 12, FontStyle.Regular);

		txtCPad.KeyUp+=new KeyEventHandler(TextBox_KeyUp);
		txtCPad.KeyPress+=new KeyPressEventHandler(TextBox_KeyPress);
		txtCPad.MouseDown+=new MouseEventHandler(TextBox_MouseDown);
		txtCPad.TextChanged+=new EventHandler(TextBox_Change);

		

		mnuMain=new MainMenu();
		
		MainMenu();
        FileMenu();
		EditMenu();
		FormatMenu();
		HelpMenu();

		this.Controls.Add(txtCPad);
		this.Controls.Add(sbCPad);
		

		this.Menu=mnuMain;
		this.Text="Untitled";
		this.WindowState=FormWindowState.Maximized;
	}
	
	//To load MainMenu Entries
	private void MainMenu()
	{
		
		mnuFile=new MenuItem();
		mnuFile.Text="&File";

		mnuEdit=new MenuItem();
		mnuEdit.Text="&Edit";

		mnuFormat=new MenuItem();
		mnuFormat.Text="&Format";

		mnuHelp=new MenuItem();
		mnuHelp.Text="&Help";

		mnuMain.MenuItems.Add(mnuFile);
		mnuMain.MenuItems.Add(mnuEdit);
		mnuMain.MenuItems.Add(mnuFormat);
		mnuMain.MenuItems.Add(mnuHelp);
	}

	private void FileMenu()
	{
		string[] MenuCaption={"New","Open","Save","Save As","Exit"};
		Shortcut[] key={Shortcut.CtrlN,Shortcut.CtrlO,Shortcut.CtrlS,Shortcut.None,Shortcut.CtrlX};

		for(int i=0;i<MenuCaption.Length;i++)
		{
			MenuItem mnuItem=new MenuItem();
			mnuItem.Text=MenuCaption[i];
			mnuItem.Click+=new EventHandler(Menu_Click);
			mnuItem.Shortcut=key[i];
			mnuFile.MenuItems.Add(mnuItem);
		}
	}
	private void EditMenu()
	{
		string[] MenuCaption={"Undo","Cut","Copy","Paste","Delete","Find","Find Next","Go To","Select All","Time/Date"};
		Shortcut[] key={Shortcut.CtrlZ,Shortcut.CtrlX,Shortcut.CtrlC,
						Shortcut.CtrlV,Shortcut.Del,Shortcut.CtrlF,Shortcut.F3,Shortcut.CtrlH,
						Shortcut.CtrlG,Shortcut.CtrlA,Shortcut.F5};
		for(int i=0;i<MenuCaption.Length;i++)
		{
			MenuItem mnuItem=new MenuItem();
			mnuItem.Text=MenuCaption[i];
			mnuItem.Click+=new EventHandler(Menu_Click);
			mnuItem.Shortcut=key[i];
			mnuEdit.MenuItems.Add(mnuItem);
		}
	}
	private void FormatMenu()
	{
		string[] MenuCaption={"WordWrap","Font","Image Viewer"};
		Console.WriteLine(MenuCaption[0]);
		for(int i=0;i<MenuCaption.Length;i++)
		{
			MenuItem mnuItem=new MenuItem();
			mnuItem.Text=MenuCaption[i];
			if(i==0)
			{
				mnuItem.Checked=true;
				txtCPad.WordWrap=true;
				
			}
			mnuItem.Click+=new EventHandler(Menu_Click);
			mnuFormat.MenuItems.Add(mnuItem);
		}
	}
	private void HelpMenu()
	{
		string[] MenuCaption={"About","User Manual"};
		for(int i=0;i<MenuCaption.Length;i++)
		{
			MenuItem mnuItem=new MenuItem();
			mnuItem.Text=MenuCaption[i];
			mnuItem.Click+=new EventHandler(Menu_Click);
			mnuHelp.MenuItems.Add(mnuItem);
		}
 
	}
		//To Load StatusBar Panels
	private void Statusbar()
	{
	string[] strToolTip={"Line:1","Col:1","","Time"};
	for(int i=0;i<strToolTip.Length;i++)
			{
				StatusBarPanel sbPanel=new StatusBarPanel();
				sbPanel.Text=strToolTip[i];
				sbPanel.BorderStyle=StatusBarPanelBorderStyle.Raised;
				if(strToolTip[i].Length==0)
					sbPanel.Width=500;
				sbCPad.Panels.Add(sbPanel);
				if(strToolTip[i]=="Time")
					Timer_Tick(null,null);
			}
			tmrCPad=new Timer();
			tmrCPad.Interval=1000;
			tmrCPad.Tick+=new EventHandler(Timer_Tick);
			tmrCPad.Start();
		}


	//To Show Time in StatusBar
	private void Timer_Tick(object sender,EventArgs eArgs)
	{
		DateTime CurrentTime=DateTime.Now;
		int h=CurrentTime.Hour;
		int m=CurrentTime.Minute;
		int s=CurrentTime.Second;
		string TimeInString=(h>9)?h.ToString():"0" + h.ToString();
		TimeInString+=":";
		TimeInString+=(m>9)?m.ToString():"0" + m.ToString();
		TimeInString+=":";
		TimeInString+=(s>9)?s.ToString():"0" + s.ToString();
		sbCPad.Panels[sbCPad.Panels.Count-1].Text=TimeInString;
	}
	public void Menu_Click(object sender,EventArgs eArgs)
	{
		string MenuCaption=((MenuItem)sender).Text;
		switch(MenuCaption)
		{
			case "New":
				DoNew();
				break;
			case "Open":
				DoOpen();
				break;
			case "Save":
				DoSave();
				break;
			case "Save As":
				DoSaveAs(true);
				break;
			case "Print":
				break;
			case "PageSetup":
				break;
			case "Exit":
				this.Dispose();
				Application.Exit();
				break;
			case "Undo":
				DoUndo();
				break;
			case "Cut":
				DoCut();
				break;
			case "Copy":
				DoCopy();
				break;
			case "Paste":
				DoPaste();
				break;
			case "Delete":
				if(txtCPad.SelectionLength==0)
					txtCPad.SelectionLength=1;
				txtCPad.SelectedText="";
				break;
			case "Find":
				ShowFind();
				break;
			case "Find Next":
				Find_Click(null,null);
				break;
			case "Replace":
				ShowReplace();
				break;
			case "Go To":
				int LineNumber=GoTo(sbCPad.Panels[0].Text.Substring(6).Trim());
				if(LineNumber>0)
				{
					MoveToLine(LineNumber);
				}
				else
				{
					if(LineNumber<0)
						MessageBox.Show("Invalid Line Number.");          
				}
				break;
			case "Select All":
				txtCPad.SelectionStart=0;
				txtCPad.SelectionLength=txtCPad.Text.Length;
				break;
			case "Time/Date":
				txtCPad.SelectedText=DateTime.Now.ToString();
				break;
			case "WordWrap":
				((MenuItem)sender).Checked=!((MenuItem)sender).Checked;
				txtCPad.WordWrap=((MenuItem)sender).Checked;
				break;
			case "Font":
				SetFont();
				break;
			case "About":
				MessageBox.Show("iText\n R.Indumathi\n Mini Project\n Email:indu_ashwini@rediffmail.com\nSona College of Technology");
				break;
			case "User Manual":
				MessageBox.Show("The User Manual is present in the folder containing the software");
				break;
			case "Image Viewer":
				Form1 f=new Form1();
				f.Show();
				break;
		}
	}

	public void MoveToLine(int LineNumber)
	{
		int CRPosition=txtCPad.Text.IndexOf("\n");
		int LineCount=0;
		bool IsMoved=false;
		while(CRPosition >-1 && !IsMoved)       
		{
			LineCount++;
			if(LineNumber==LineCount)
			{
				txtCPad.SelectionStart=CRPosition-1;
				TextBox_KeyUp(null,null);
				IsMoved=true;
			}
			CRPosition=txtCPad.Text.IndexOf("\n",CRPosition+1);
		}
		if(!IsMoved)
		{
			if(LineNumber==LineCount+1)
				txtCPad.SelectionStart=txtCPad.Text.Length;
			else
				MessageBox.Show("Line Number Out of Range");
		}
	}

	private void DoCut()
	{
		if(txtCPad.SelectedText!="")
		{
			txtCPad.Cut();
			TextBox_KeyUp(null,null);
		}

	}

	private void DoCopy()
	{
		if(txtCPad.SelectedText!="")
		{
			flag=true;
			txtCPad.Copy();
			TextBox_KeyUp(null,null);
		}
	}

	private void DoPaste()
	{
		if(flag){
				txtCPad.Paste();
				TextBox_KeyUp(null,null);
			}   
		
	}

	private void DoUndo()
	{
		if(txtCPad.CanUndo)
		{
			txtCPad.Undo();
			TextBox_KeyUp(null,null);
		}
	}

	private void DoRedo()
	{
		if(txtCPad.CanUndo)
		{
			txtCPad.Undo();
			TextBox_KeyUp(null,null);
		}
	}


	public void DoSaveAs(bool ShowDialog)
	{
		bool CanWrite=false;
		SaveFileDialog  dlgSave=new SaveFileDialog();
		dlgSave.Filter="Text Files (*.txt)|*.txt|C# Files|*.cs|All Files (*.*)|*.*";
		dlgSave.FilterIndex=2;
		dlgSave.RestoreDirectory=true;
		string FileName="";
		if(ShowDialog)
		{
			if(dlgSave.ShowDialog()==DialogResult.OK)
			{
				FileName=dlgSave.FileName;
				CanWrite=true;
			}
		}
		else
		{
			FileName=this.Text;
			CanWrite=true;
		}
		if(CanWrite)
		{
			try
			{
				StreamWriter sw=File.CreateText(FileName);
				sw.Write(txtCPad.Text);
				sw.Flush();
				sw.Close();
				this.Text=FileName;
			}
			catch(Exception e)
			{ 
				MessageBox.Show(e.ToString());
			}
		}

	}

	public void DoSave()
	{
		if(this.Text=="Untitled")
		{
			DoSaveAs(true);
		}
		DoSaveAs(false);
		IsContentModified=false;
	}

	private void DoNew()
	{
		if(IsContentModified)
		{
			string Message="The text in the " + this.Text + " has been changed.\n Do you want to Save Changes?";      
			switch(MessageBox.Show(Message,"SuperPad",MessageBoxButtons.YesNoCancel))
			{
				case DialogResult.Yes:
					DoSave();
					txtCPad.Text="";
					this.Text="Untitled";                    
					break;
				case DialogResult.No:
					txtCPad.Text="";
					this.Text="Untitled";                    
					break;
				case DialogResult.Cancel:
					break;
			}
		}
		else
		{
			txtCPad.Text="";
			this.Text="Untitled";
		}
	}

	private void DoOpen()
	{
		OpenFileDialog dlgOpen=new OpenFileDialog();
		dlgOpen.Filter="All files (*.*)|*.*|All files (*.*)|*.*";
		dlgOpen.FilterIndex=2;
		dlgOpen.RestoreDirectory=true;
		string FileName="";
		if(dlgOpen.ShowDialog()==DialogResult.OK)
		{
			try
			{
				FileName=dlgOpen.FileName;
				FileStream fs=File.OpenRead(FileName);
				
				txtCPad.Text="";
				string strLine="";
				int c;
				while((c=fs.ReadByte())!=-1)
				{
					strLine+=(char)c;
				}
				txtCPad.Text+=strLine+"\n";
				txtCPad.SelectionStart=0;
				txtCPad.SelectionLength=0;
				this.Text=FileName;
				fs.Close();
				
			}
			catch(Exception e)
			{
				MessageBox.Show("Error Occurred:\n" + e.ToString());
			}
		}
	}

	public void SetFont()
	{
		FontDialog dlgFont=new FontDialog();
		dlgFont.Font=txtCPad.Font;
		dlgFont.ShowColor=true;
		dlgFont.Color=txtCPad.ForeColor;
		if(dlgFont.ShowDialog()==DialogResult.OK)
		{
			txtCPad.Font=dlgFont.Font;
			txtCPad.ForeColor=dlgFont.Color;
		}
	}

	public int GoTo(string LineNumber)
	{
		Form frmDialog=new Form();

		TextBox LineBox=new TextBox();
		Button cmdOK=new Button();
		Button cmdCancel=new Button();

		cmdOK.Text="OK";
		cmdOK.DialogResult=DialogResult.OK;
		cmdOK.Click-=new EventHandler(Find_Click);

		cmdCancel.Text="Cancel";
		cmdCancel.DialogResult=DialogResult.Cancel;
		LineBox.Text=LineNumber;
		if(LineBox.Text.Length==0) LineBox.Text="0";
		frmDialog.Size=new Size(230,100);
		frmDialog.Text="Go to Line";
		frmDialog.AcceptButton=cmdOK;
		frmDialog.CancelButton=cmdCancel;
		frmDialog.MaximizeBox=false;
		frmDialog.MinimizeBox=false;
		frmDialog.FormBorderStyle=FormBorderStyle.FixedDialog;
		frmDialog.Location=new Point(this.Left+Math.Abs((this.Width-frmDialog.Width))/2,this.Top + 100);

		LineBox.Location=new Point(10,10);
		cmdOK.Location=new Point(LineBox.Left+LineBox.Width+20,10);
		cmdCancel.Location=new Point(cmdOK.Left,cmdOK.Top+cmdOK.Height+10);

		frmDialog.Controls.Add(LineBox);
		frmDialog.Controls.Add(cmdOK);
		frmDialog.Controls.Add(cmdCancel);
		frmDialog.ShowDialog(this);

		if(frmDialog.DialogResult==DialogResult.OK)
		{
			try
			{
				frmDialog.Dispose();
				return Int32.Parse(LineBox.Text);
			}
			catch(Exception)
			{
				return -1;
			}
		}
		return 0;
	}

	private void ShowFind()
	{
		TextBox txtFind=new TextBox();
		Label lblFind=new Label();
		CheckBox chkMatch=new CheckBox();
		GroupBox grpDirection=new GroupBox();
		RadioButton optUp=new RadioButton();
		RadioButton optDown=new RadioButton();
		Button cmdOK=new Button();
		Button cmdCancel=new Button();

		cmdOK.Text="Find Next";
		cmdOK.DialogResult=DialogResult.OK;
		cmdOK.Click+=new EventHandler(Find_Click);

		cmdCancel.Text="Cancel";
		cmdCancel.DialogResult=DialogResult.Cancel;
		cmdCancel.Click+=new EventHandler(Cancel_Click);

		Form frmDialog=new Form();
		frmDialog.Size=new Size(380,150);
		frmDialog.Text="Find";
		frmDialog.MaximizeBox=false;
		frmDialog.MinimizeBox=false;
		frmDialog.FormBorderStyle=FormBorderStyle.FixedDialog;

		lblFind.Location=new Point(5,15);
		lblFind.Text="Fi&nd what:";
		lblFind.AutoSize=true;
    
		txtFind.Location=new Point(lblFind.Left+lblFind.Width+5,lblFind.Top);
		txtFind.Size=new Size(200,txtFind.Height);
		txtFind.Text=TextToFind;
		txtFind.TextChanged+=new EventHandler(txtFind_Change);

		cmdOK.Location=new Point(txtFind.Left+txtFind.Width+5,lblFind.Top-2);
		cmdCancel.Location=new Point(cmdOK.Left,cmdOK.Top+cmdOK.Height+5);

		grpDirection.Location=new Point(lblFind.Left,cmdCancel.Top+cmdCancel.Height);
		grpDirection.Size=new Size(120,45);
		grpDirection.Text="Match Direction";

		optUp.Location=new Point(10,15);
		optUp.Click+=new EventHandler(Direction_Click);
		optUp.Text="&Up";
		optUp.Width=45;

		optDown.Text="&Down";
		optDown.Width=55;
		optDown.Click+=new EventHandler(Direction_Click);
		optDown.Checked=true;
		optDown.Location=new Point(optUp.Left+optUp.Width,optUp.Top);

		grpDirection.Controls.Add(optUp);
		grpDirection.Controls.Add(optDown);

		chkMatch.Location=new Point(grpDirection.Left+grpDirection.Width+5,grpDirection.Top+(grpDirection.Height-chkMatch.Height)/2);
		chkMatch.Text="Match Case";

		if(txtFind.Text.Trim().Length==0) cmdOK.Enabled=false;

		frmDialog.Controls.Add(txtFind);
		frmDialog.Controls.Add(lblFind);
		frmDialog.Controls.Add(grpDirection);
		frmDialog.Controls.Add(cmdOK);
		frmDialog.Controls.Add(cmdCancel);
		frmDialog.ShowInTaskbar=false;
		frmDialog.TopMost=true;
		frmDialog.Show();
	}

	private void ShowReplace()
	{
		TextBox txtFind=new TextBox();
		TextBox txtReplace=new TextBox();
		Label lblFind=new Label();
		Label lblReplace=new Label();
		CheckBox chkMatch=new CheckBox();
		GroupBox grpDirection=new GroupBox();
		RadioButton optUp=new RadioButton();
		RadioButton optDown=new RadioButton();
		Button cmdOK=new Button();
		Button cmdReplaceALL=new Button();
		Button cmdCancel=new Button();

		cmdOK.Text="Replace";
		cmdOK.DialogResult=DialogResult.OK;
		cmdOK.Click+=new EventHandler(Replace_Click);

		cmdReplaceALL.Text="Replace All";
		cmdReplaceALL.DialogResult=DialogResult.OK;
		cmdReplaceALL.Click+=new EventHandler(ReplaceALL_Click);

		cmdCancel.Text="Cancel";
		cmdCancel.DialogResult=DialogResult.Cancel;
		cmdCancel.Click+=new EventHandler(Cancel_Click);

		Form frmDialog=new Form();
		frmDialog.Size=new Size(380,200);
		frmDialog.Text="Replace";
		frmDialog.MaximizeBox=false;
		frmDialog.MinimizeBox=false;
		frmDialog.FormBorderStyle=FormBorderStyle.FixedDialog;

		lblFind.Location=new Point(5,15);
		lblFind.Text="Find:";
		lblFind.AutoSize=true;
    
		txtFind.Location=new Point(lblFind.Left+lblFind.Width+5,lblFind.Top);
		txtFind.Size=new Size(200,txtFind.Height);
		txtFind.Text=TextToFind;
		txtFind.TextChanged+=new EventHandler(txtFind_Change);

		lblReplace.Location=new Point(lblFind.Left,lblFind.Top+lblFind.Height+15);
		lblReplace.Text="Replace:";
		lblReplace.AutoSize=true;
    
		txtReplace.Location=new Point(lblReplace.Left+lblReplace.Width+5,lblReplace.Top);
		txtReplace.Size=new Size(200,txtFind.Height);
		txtFind.Left=txtReplace.Left;
		txtReplace.Text=TextToReplace;
		txtReplace.TextChanged+=new EventHandler(txtReplace_Change);
    

		cmdOK.Location=new Point(txtFind.Left+txtFind.Width+5,lblFind.Top-2);
		cmdReplaceALL.Location=new Point(cmdOK.Left,cmdOK.Top+cmdOK.Height+5);
		cmdCancel.Location=new Point(cmdReplaceALL.Left,cmdReplaceALL.Top+cmdReplaceALL.Height+5);

		grpDirection.Location=new Point(lblReplace.Left,cmdCancel.Top+cmdCancel.Height);
		grpDirection.Size=new Size(120,45);
		grpDirection.Text="Match Direction";

		optUp.Location=new Point(10,15);
		optUp.Click+=new EventHandler(Direction_Click);
		optUp.Text="&Up";
		optUp.Width=45;

		optDown.Text="&Down";
		optDown.Width=55;
		optDown.Click+=new EventHandler(Direction_Click);
		optDown.Checked=true;
		optDown.Location=new Point(optUp.Left+optUp.Width,optUp.Top);

		grpDirection.Controls.Add(optUp);
		grpDirection.Controls.Add(optDown);

		chkMatch.Location=new Point(grpDirection.Left+grpDirection.Width+5,grpDirection.Top+(grpDirection.Height-chkMatch.Height)/2);
		chkMatch.Text="Match Case";

		if(txtFind.Text.Trim().Length==0)
		{
			cmdOK.Enabled=false;
			cmdReplaceALL.Enabled=false;
		}

		frmDialog.Controls.Add(txtFind);
		frmDialog.Controls.Add(lblFind);
		frmDialog.Controls.Add(txtReplace);
		frmDialog.Controls.Add(lblReplace);
		frmDialog.Controls.Add(grpDirection);
		frmDialog.Controls.Add(cmdOK);
		frmDialog.Controls.Add(cmdReplaceALL);
		frmDialog.Controls.Add(cmdCancel);
		frmDialog.ShowInTaskbar=false;
		frmDialog.TopMost=true;
		frmDialog.Show();
	}

	private void txtFind_Change(object sender,EventArgs eArgs)
	{    
		TextToFind=((TextBox)sender).Text;
		Form frmTemp=(Form)(((Control)sender).Parent);
		for(int i=0;i<frmTemp.Controls.Count;i++)
		{
			if(frmTemp.Controls[i].GetType() ==typeof(Button) && frmTemp.Controls[i].Text!="&Cancel")
			{
				frmTemp.Controls[i].Enabled=(((TextBox)sender).Text.Length>0);
			}
		}
		frmTemp=null;
	}

	private void txtReplace_Change(object sender,EventArgs eArgs)
	{    
		TextToReplace=((TextBox)sender).Text;
	}

	private void Direction_Click(object sender,EventArgs eArgs)
	{
		string Caption=((RadioButton)sender).Text;
		if(Caption=="&Up")
			IsDirectionDownward=false;
		else
			IsDirectionDownward=true;
	}

	private void Find_Click(object sender,EventArgs eArgs)
	{
		int Pos=-1;

		if(IsDirectionDownward)
		{
			if(txtCPad.SelectionLength==0)
				Pos=txtCPad.Text.IndexOf(TextToFind,txtCPad.SelectionStart);
			else
				Pos=txtCPad.Text.IndexOf(TextToFind,txtCPad.SelectionStart+txtCPad.SelectionLength);
		}
		else
		{
			if(txtCPad.SelectionStart>0)

				Pos=txtCPad.Text.LastIndexOf(TextToFind,txtCPad.SelectionStart-1);
		}
		if(Pos!=-1)
		{
			txtCPad.SelectionStart=Pos;
			txtCPad.SelectionLength=TextToFind.Length;
			if(sender!=null) ((Control)sender).Focus();
		}
		else
		{
			MessageBox.Show("Cannot Find: \"" + TextToFind + "\"");
		}
	}

	private void Replace_Click(object sender,EventArgs eArgs)
	{
		Find_Click(null,null);
		if(txtCPad.SelectionLength>0)
		{
			txtCPad.SelectedText=TextToReplace;
			txtCPad.SelectionStart=txtCPad.SelectionStart+TextToReplace.Length;
		}
	}

	private void ReplaceALL_Click(object sender,EventArgs eArgs)
	{
		txtCPad.Text=Replace(txtCPad.Text,TextToFind,TextToReplace);
	}

	private string Replace(string StrSource,string StrFind,string StrReplace)
	{
		int iPos=StrSource.IndexOf(StrFind);
		String StrReturn="";

		while(iPos!=-1)
		{
			StrReturn+=StrSource.Substring(0,iPos)+StrReplace;
			StrSource=StrSource.Substring(iPos+StrFind.Length);
			iPos=StrSource.IndexOf(StrFind);
		}
		if(StrSource.Length>0)
			StrReturn+=StrSource;
		return StrReturn;
	}

	private void Cancel_Click(object sender,EventArgs eArgs)
	{
		((Form)((Control)sender).Parent).Close();
	}
	//To set the Content modified flag

	private void TextBox_Change(object sender,EventArgs eArgs)
	{
		IsContentModified=true;
	}

	//To Set Line,Col in StatusBar
	private void TextBox_KeyUp(object sender,KeyEventArgs kArgs)
	{
		try
		{
			int ColCount=1;
			int RowCount=1;
			int Pos;
			//To Set Column 
			if(txtCPad.SelectionStart>-1)
			{
				Pos=txtCPad.Text.LastIndexOf("\n",txtCPad.SelectionStart);
				if(Pos>-1)
				{
					//If the cursor is at CRLF
					if(Pos!=txtCPad.SelectionStart)
						ColCount=txtCPad.SelectionStart-Pos;
					else
					{
						//Col position is diff between PrevEnter and CurPos
						Pos=txtCPad.Text.LastIndexOf("\n",txtCPad.SelectionStart-1);            
						ColCount=txtCPad.SelectionStart-Pos;
					}
				}
				else
				{
					ColCount=txtCPad.SelectionStart+1;
				}
				while(Pos>-1)
				{
					RowCount++;
					Pos=txtCPad.Text.LastIndexOf("\n",Pos-1);
				}
			}
			sbCPad.Panels[1].Text="Col: " + ColCount.ToString();
			sbCPad.Panels[0].Text="Line: " + RowCount.ToString();
		}
		catch(Exception)
		{
		}
	}

	//To Set Line,Col in StatusBar
	private void TextBox_KeyPress(object sender,KeyPressEventArgs kpArgs)
	{
		TextBox_KeyUp(null,null);
	}

	//To Set Line,Col in StatusBar
	private void TextBox_MouseDown(object sender,MouseEventArgs mArgs)
	{
		TextBox_KeyUp(null,null);
	}
	
	public static void Main()
	{
		Class1 c=new Class1();
		Application.Run(new CPad()); //Calling the Run method and passing  
		//	a new instance of the class as argument
	}
}

//ImageViewer
public class Form1:System.Windows.Forms.Form {

		private System.ComponentModel.Container components;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;

		public Form1()
		{
			InitializeComponent();
		}

		public void InitializeComponent()
		{
			this.components=new System.ComponentModel.Container();
			this.button3=new System.Windows.Forms.Button();
			this.button2=new System.Windows.Forms.Button();
			this.button1=new System.Windows.Forms.Button();

			button2.Location=new System.Drawing.Point(16,40);
			button2.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			button2.Size=new System.Drawing.Size(408,296);
			button2.TabIndex=1;

			button3.Location=new System.Drawing.Point(200,8);
			button3.Size=new System.Drawing.Size(72,24);
			button3.TabIndex=3;
			button3.Text="&Close";
			button3.Click+=new System.EventHandler(this.button3_Click);

			button1.Location=new System.Drawing.Point(336,8);
			button1.Size=new System.Drawing.Size(80,24);
			button1.TabIndex=2;
			button1.Text="Browse";
			button1.Click+=new System.EventHandler(this.button1_Click);

			this.Text="iText Image Viewer";
			this.AutoScaleBaseSize=new System.Drawing.Size(5,13);
			this.ClientSize=new System.Drawing.Size(448,357);

			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);

		}

		protected void button3_Click(object sender,System.EventArgs e)
		{
			this.Close();
		}
		protected void button1_Click(object sender,System.EventArgs e)
		{
			OpenFileDialog fdlg=new OpenFileDialog();
			fdlg.Title="C# Projects Open File Dialog";
			fdlg.InitialDirectory=@"C:\";
			fdlg.Filter="All files (*.*)|*.*|All files (*.*)|*.*";
			fdlg.FilterIndex=2;
			fdlg.RestoreDirectory=true;

			if(fdlg.ShowDialog()==DialogResult.OK)
			{
				button2.Image=Image.FromFile(fdlg.FileName);
			}
			Invalidate();
		}
	
		
	}

public class Class1 
{
	static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
	static bool exitFlag = false;
	Form2 f;
	
	// This is the method to run when the timer is raised.
	private static void TimerEventProcessor(Object myObject,
		EventArgs myEventArgs) 

	{
		
		
		exitFlag=true;
		myTimer.Stop();
	}
 
	public Class1() 
	{
		/* Adds the event and the event handler for the method that will 
		   process the timer event to the timer. */
		myTimer.Tick += new EventHandler(TimerEventProcessor);
 
		// Sets the timer interval to 5 seconds.
		myTimer.Interval = 5000;
		f=new Form2();
		f.Show();
		myTimer.Start();
		// Runs the timer, and raises the event.
		while(exitFlag == false)
		{
		Application.DoEvents();
			// Processes all the events in the queue.
		}
		f.Close();
		//return 0;
	}
}


class Form2 : System.Windows.Forms.Form
{
	private System.Windows.Forms.PictureBox pictureBox1;
	private System.ComponentModel.Container components = null;
	
	public Form2()
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();
		string path="iText.jpg";
		pictureBox1.Image = Image.FromFile(path);
	}
	protected override void Dispose( bool disposing )
	{
		if( disposing )
		{
			if (components != null) 
			{
				components.Dispose();
			}
		}
		base.Dispose( disposing );
	}

	private void InitializeComponent()
	{
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.SuspendLayout();
		// 
		// pictureBox1
		// 
		//this.pictureBox1.Location = new System.Drawing.Point(0,-8);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(600, 280);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		//this.pictureBox1.Size = new System.Drawing.Size(900,900);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		// 
		// Form1
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(600, 300);
		this.Controls.AddRange(new System.Windows.Forms.Control[] {
																	  this.pictureBox1});
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		this.Name = "Form1";
		this.Text = "iText - Splash Screen";
		
		this.ResumeLayout(false);

	}
		
	}



