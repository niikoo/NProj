<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Menu1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Menu1))
        Me.Header = New System.Windows.Forms.Label
        Me.player2 = New System.Windows.Forms.Button
        Me.player1 = New System.Windows.Forms.Button
        Me.Om = New System.Windows.Forms.Button
        Me.Avslutt = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Header
        '
        Me.Header.BackColor = System.Drawing.Color.Transparent
        Me.Header.Font = New System.Drawing.Font("Calibri", 18.25!)
        Me.Header.ForeColor = System.Drawing.Color.White
        Me.Header.Location = New System.Drawing.Point(12, 9)
        Me.Header.Name = "Header"
        Me.Header.Size = New System.Drawing.Size(142, 31)
        Me.Header.TabIndex = 43
        Me.Header.Text = "TicTacToe"
        Me.Header.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'player2
        '
        Me.player2.BackColor = System.Drawing.Color.Transparent
        Me.player2.BackgroundImage = Global.TicTacToe.My.Resources.Resources.langbar_big_green
        Me.player2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.player2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.player2.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.player2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.player2.Location = New System.Drawing.Point(12, 94)
        Me.player2.Name = "player2"
        Me.player2.Size = New System.Drawing.Size(142, 23)
        Me.player2.TabIndex = 44
        Me.player2.Text = "2 Spillere"
        Me.player2.UseVisualStyleBackColor = False
        '
        'player1
        '
        Me.player1.BackColor = System.Drawing.Color.Transparent
        Me.player1.BackgroundImage = Global.TicTacToe.My.Resources.Resources.langbar_big
        Me.player1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.player1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.player1.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.player1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.player1.Location = New System.Drawing.Point(12, 65)
        Me.player1.Name = "player1"
        Me.player1.Size = New System.Drawing.Size(142, 23)
        Me.player1.TabIndex = 45
        Me.player1.Text = "1 Spiller"
        Me.player1.UseVisualStyleBackColor = False
        '
        'Om
        '
        Me.Om.BackColor = System.Drawing.Color.Transparent
        Me.Om.BackgroundImage = Global.TicTacToe.My.Resources.Resources.langbar_big_yellow
        Me.Om.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Om.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Om.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Om.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Om.Location = New System.Drawing.Point(12, 123)
        Me.Om.Name = "Om"
        Me.Om.Size = New System.Drawing.Size(142, 23)
        Me.Om.TabIndex = 46
        Me.Om.Text = "Om"
        Me.Om.UseVisualStyleBackColor = False
        '
        'Avslutt
        '
        Me.Avslutt.BackColor = System.Drawing.Color.Transparent
        Me.Avslutt.BackgroundImage = Global.TicTacToe.My.Resources.Resources.langbar_big_rød
        Me.Avslutt.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Avslutt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Avslutt.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Avslutt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Avslutt.Location = New System.Drawing.Point(12, 152)
        Me.Avslutt.Name = "Avslutt"
        Me.Avslutt.Size = New System.Drawing.Size(142, 23)
        Me.Avslutt.TabIndex = 47
        Me.Avslutt.Text = "Avlsutt"
        Me.Avslutt.UseVisualStyleBackColor = False
        '
        'Menu1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.TicTacToe.My.Resources.Resources._890579818_6901fc666c_b
        Me.ClientSize = New System.Drawing.Size(166, 185)
        Me.Controls.Add(Me.Avslutt)
        Me.Controls.Add(Me.Om)
        Me.Controls.Add(Me.player1)
        Me.Controls.Add(Me.player2)
        Me.Controls.Add(Me.Header)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Menu1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TicTacToe - Menu"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Header As System.Windows.Forms.Label
    Friend WithEvents player2 As System.Windows.Forms.Button
    Friend WithEvents player1 As System.Windows.Forms.Button
    Friend WithEvents Om As System.Windows.Forms.Button
    Friend WithEvents Avslutt As System.Windows.Forms.Button
End Class
