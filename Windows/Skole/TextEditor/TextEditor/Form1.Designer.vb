<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FilToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ÅpneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LagreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LagreSomToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AvsluttToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.HjelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OmToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FilToolStripMenuItem, Me.HjelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.MenuStrip1.Size = New System.Drawing.Size(672, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FilToolStripMenuItem
        '
        Me.FilToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÅpneToolStripMenuItem, Me.LagreToolStripMenuItem, Me.LagreSomToolStripMenuItem, Me.AvsluttToolStripMenuItem})
        Me.FilToolStripMenuItem.Name = "FilToolStripMenuItem"
        Me.FilToolStripMenuItem.Size = New System.Drawing.Size(29, 20)
        Me.FilToolStripMenuItem.Text = "Fil"
        '
        'ÅpneToolStripMenuItem
        '
        Me.ÅpneToolStripMenuItem.Name = "ÅpneToolStripMenuItem"
        Me.ÅpneToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ÅpneToolStripMenuItem.Text = "Åpne"
        '
        'LagreToolStripMenuItem
        '
        Me.LagreToolStripMenuItem.Name = "LagreToolStripMenuItem"
        Me.LagreToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LagreToolStripMenuItem.Text = "Lagre"
        '
        'LagreSomToolStripMenuItem
        '
        Me.LagreSomToolStripMenuItem.Name = "LagreSomToolStripMenuItem"
        Me.LagreSomToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LagreSomToolStripMenuItem.Text = "Lagre som.."
        '
        'AvsluttToolStripMenuItem
        '
        Me.AvsluttToolStripMenuItem.Name = "AvsluttToolStripMenuItem"
        Me.AvsluttToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AvsluttToolStripMenuItem.Text = "Avslutt"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "*.*"
        Me.OpenFileDialog1.Filter = "Tekstfiler txt/rtf |*.txt;*.rtf|Konfigurasjonsfiler ini/inf|*.inf;*.ini|Alle file" & _
            "r|*.*"
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox1.EnableAutoDragDrop = True
        Me.RichTextBox1.Location = New System.Drawing.Point(0, 24)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(672, 519)
        Me.RichTextBox1.TabIndex = 1
        Me.RichTextBox1.Text = ""
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.DefaultExt = "*.txt"
        Me.SaveFileDialog1.Filter = "Tekstdokument *.txt|*.txt|Alle Filer|*.*"
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TextBox1.Location = New System.Drawing.Point(0, 523)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(672, 20)
        Me.TextBox1.TabIndex = 2
        '
        'HjelpToolStripMenuItem
        '
        Me.HjelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OmToolStripMenuItem})
        Me.HjelpToolStripMenuItem.Name = "HjelpToolStripMenuItem"
        Me.HjelpToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.HjelpToolStripMenuItem.Text = "Hjelp"
        '
        'OmToolStripMenuItem
        '
        Me.OmToolStripMenuItem.Name = "OmToolStripMenuItem"
        Me.OmToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OmToolStripMenuItem.Text = "Om"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(672, 543)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Nikolai's Notepad"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FilToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AvsluttToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ÅpneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents LagreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents LagreSomToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents HjelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OmToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
