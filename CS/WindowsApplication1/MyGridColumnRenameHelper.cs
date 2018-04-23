using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.Skins;

namespace WindowsApplication1
{
    public class MyGridColumnRenameHelper
    {
        #region Fields

        GridView gridView;
        TextEdit headerEdit;
        GridColumn editedColumn;

        #endregion

        public bool IsEditing
        {
            get { return editedColumn != null; }
        }


        #region Methods
        public MyGridColumnRenameHelper(GridView view)
        {
            gridView = view;
            Initialize();
            SubscribeEvents();
        }

        void SubscribeEvents()
        {
            gridView.DoubleClick += gridView_DoubleClick;
            headerEdit.Leave += headerEdit_Leave;
            headerEdit.KeyDown += headerEdit_KeyDown;
        }

        GridColumn GetColumn(DXMouseEventArgs args)
        {
            GridHitInfo info = gridView.CalcHitInfo(args.Location);
            if (info.InColumnPanel) return info.Column;
            return null;
        }

        Color GetColor()
        {
            Skin currentSkin = CommonSkins.GetSkin(gridView.GridControl.LookAndFeel);
            return currentSkin.TranslateColor(SystemColors.Control);
        }

        void Initialize()
        {
            gridView.OptionsCustomization.AllowSort = false;
            headerEdit = new TextEdit();
            headerEdit.Hide();
            headerEdit.Parent = gridView.GridControl;
            headerEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        void ShowCaptionEditor(GridColumn column)
        {
            GridViewInfo vi = gridView.GetViewInfo() as GridViewInfo;
            Rectangle bounds = vi.ColumnsInfo[column].Bounds;
            bounds.Width -= 3;
            bounds.Height -= 3;
            bounds.Y += 3;
            headerEdit.BackColor = GetColor();
            headerEdit.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            headerEdit.EditValue = column.GetCaption();
            headerEdit.Show();
            headerEdit.Focus();
        }

        void StartColumnCaptionEditing(GridColumn column)
        {
            ShowCaptionEditor(column);
            editedColumn = column;
        }

        void EndColumnCaptionEditing()
        {
            if (!IsEditing) return;
            editedColumn.Caption = headerEdit.Text;
            headerEdit.Hide();
            editedColumn = null;
        }
        #endregion


        void gridView_DoubleClick(object sender, EventArgs e)
        {
            GridColumn column = GetColumn(e as DXMouseEventArgs);
            if (column == null) return;
            StartColumnCaptionEditing(column);
        }


        void headerEdit_Leave(object sender, EventArgs e)
        {
            EndColumnCaptionEditing();
        }


        void headerEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                EndColumnCaptionEditing();
        }

    
    }
}
