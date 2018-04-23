Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraEditors
Imports DevExpress.Skins

Namespace WindowsApplication1
	Public Class MyGridColumnRenameHelper
		#Region "Fields"

		Private gridView As GridView
		Private headerEdit As TextEdit
		Private editedColumn As GridColumn

		#End Region

		Public ReadOnly Property IsEditing() As Boolean
			Get
				Return editedColumn IsNot Nothing
			End Get
		End Property


		#Region "Methods"
		Public Sub New(ByVal view As GridView)
			gridView = view
			Initialize()
			SubscribeEvents()
		End Sub

		Private Sub SubscribeEvents()
			AddHandler gridView.DoubleClick, AddressOf gridView_DoubleClick
			AddHandler headerEdit.Leave, AddressOf headerEdit_Leave
			AddHandler headerEdit.KeyDown, AddressOf headerEdit_KeyDown
		End Sub

		Private Function GetColumn(ByVal args As DXMouseEventArgs) As GridColumn
			Dim info As GridHitInfo = gridView.CalcHitInfo(args.Location)
			If info.InColumnPanel Then
				Return info.Column
			End If
			Return Nothing
		End Function

		Private Function GetColor() As Color
			Dim currentSkin As Skin = CommonSkins.GetSkin(gridView.GridControl.LookAndFeel)
			Return currentSkin.TranslateColor(SystemColors.Control)
		End Function

		Private Sub Initialize()
			gridView.OptionsCustomization.AllowSort = False
			headerEdit = New TextEdit()
			headerEdit.Hide()
			headerEdit.Parent = gridView.GridControl
			headerEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		End Sub

		Private Sub ShowCaptionEditor(ByVal column As GridColumn)
			Dim vi As GridViewInfo = TryCast(gridView.GetViewInfo(), GridViewInfo)
			Dim bounds As Rectangle = vi.ColumnsInfo(column).Bounds
			bounds.Width -= 3
			bounds.Height -= 3
			bounds.Y += 3
			headerEdit.BackColor = GetColor()
			headerEdit.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height)
			headerEdit.EditValue = column.GetCaption()
			headerEdit.Show()
			headerEdit.Focus()
		End Sub

		Private Sub StartColumnCaptionEditing(ByVal column As GridColumn)
			ShowCaptionEditor(column)
			editedColumn = column
		End Sub

		Private Sub EndColumnCaptionEditing()
			If (Not IsEditing) Then
				Return
			End If
			editedColumn.Caption = headerEdit.Text
			headerEdit.Hide()
			editedColumn = Nothing
		End Sub
		#End Region


		Private Sub gridView_DoubleClick(ByVal sender As Object, ByVal e As EventArgs)
			Dim column As GridColumn = GetColumn(TryCast(e, DXMouseEventArgs))
			If column Is Nothing Then
				Return
			End If
			StartColumnCaptionEditing(column)
		End Sub


		Private Sub headerEdit_Leave(ByVal sender As Object, ByVal e As EventArgs)
			EndColumnCaptionEditing()
		End Sub


		Private Sub headerEdit_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.KeyData = Keys.Enter Then
				EndColumnCaptionEditing()
			End If
		End Sub


	End Class
End Namespace
