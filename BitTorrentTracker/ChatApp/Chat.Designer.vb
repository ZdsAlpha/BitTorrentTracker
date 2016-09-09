<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Chat
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Trackers = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.channel = New System.Windows.Forms.TextBox()
        Me.connectbutton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.textfield = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.message = New System.Windows.Forms.TextBox()
        Me.sendbutton = New System.Windows.Forms.Button()
        Me.checkbutton = New System.Windows.Forms.Button()
        Me.announcebutton = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Trackers:"
        '
        'Trackers
        '
        Me.Trackers.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Trackers.Location = New System.Drawing.Point(79, 38)
        Me.Trackers.Name = "Trackers"
        Me.Trackers.Size = New System.Drawing.Size(414, 20)
        Me.Trackers.TabIndex = 1
        Me.Trackers.Text = "tracker.coppersurfer.tk:6969"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Channel:"
        '
        'channel
        '
        Me.channel.Enabled = False
        Me.channel.Location = New System.Drawing.Point(79, 64)
        Me.channel.Name = "channel"
        Me.channel.Size = New System.Drawing.Size(414, 20)
        Me.channel.TabIndex = 3
        Me.channel.Text = "__tbca_default_channel"
        '
        'connectbutton
        '
        Me.connectbutton.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.connectbutton.Location = New System.Drawing.Point(499, 39)
        Me.connectbutton.Name = "connectbutton"
        Me.connectbutton.Size = New System.Drawing.Size(81, 19)
        Me.connectbutton.TabIndex = 4
        Me.connectbutton.Text = "Connect"
        Me.connectbutton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Chat:"
        '
        'textfield
        '
        Me.textfield.BackColor = System.Drawing.Color.White
        Me.textfield.Location = New System.Drawing.Point(15, 112)
        Me.textfield.Multiline = True
        Me.textfield.Name = "textfield"
        Me.textfield.ReadOnly = True
        Me.textfield.Size = New System.Drawing.Size(565, 207)
        Me.textfield.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 335)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Message:"
        '
        'message
        '
        Me.message.Enabled = False
        Me.message.Location = New System.Drawing.Point(79, 332)
        Me.message.Name = "message"
        Me.message.Size = New System.Drawing.Size(414, 20)
        Me.message.TabIndex = 8
        Me.message.Text = "Hello"
        '
        'sendbutton
        '
        Me.sendbutton.Enabled = False
        Me.sendbutton.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sendbutton.Location = New System.Drawing.Point(499, 333)
        Me.sendbutton.Name = "sendbutton"
        Me.sendbutton.Size = New System.Drawing.Size(81, 19)
        Me.sendbutton.TabIndex = 9
        Me.sendbutton.Text = "Send"
        Me.sendbutton.UseVisualStyleBackColor = True
        '
        'checkbutton
        '
        Me.checkbutton.Enabled = False
        Me.checkbutton.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.checkbutton.Location = New System.Drawing.Point(499, 65)
        Me.checkbutton.Name = "checkbutton"
        Me.checkbutton.Size = New System.Drawing.Size(81, 19)
        Me.checkbutton.TabIndex = 10
        Me.checkbutton.Text = "Check"
        Me.checkbutton.UseVisualStyleBackColor = True
        '
        'announcebutton
        '
        Me.announcebutton.Enabled = False
        Me.announcebutton.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.announcebutton.Location = New System.Drawing.Point(499, 90)
        Me.announcebutton.Name = "announcebutton"
        Me.announcebutton.Size = New System.Drawing.Size(81, 19)
        Me.announcebutton.TabIndex = 11
        Me.announcebutton.Text = "Announce"
        Me.announcebutton.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(79, 12)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(414, 20)
        Me.TextBox2.TabIndex = 12
        Me.TextBox2.Text = "unnamed"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Name:"
        '
        'Chat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(592, 368)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.announcebutton)
        Me.Controls.Add(Me.checkbutton)
        Me.Controls.Add(Me.sendbutton)
        Me.Controls.Add(Me.message)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.textfield)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.connectbutton)
        Me.Controls.Add(Me.channel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Trackers)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Chat"
        Me.Opacity = 0.95R
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tracker Based Chat App"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Trackers As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents channel As TextBox
    Friend WithEvents connectbutton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents textfield As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents message As TextBox
    Friend WithEvents sendbutton As Button
    Friend WithEvents checkbutton As Button
    Friend WithEvents announcebutton As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label5 As Label
End Class
