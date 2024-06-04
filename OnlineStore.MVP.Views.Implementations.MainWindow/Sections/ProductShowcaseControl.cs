using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;
using System.Collections.ObjectModel;

namespace OnlineStore.MVP.Views.Implementations.MainWindow.Sections;

public sealed partial class ProductShowcaseControl : UserControl, IProductShowcaseView
{
    public event Func<Task<ReadOnlyCollection<ProductModel>?>> LoadProducts;
    public event Func<ProductModel, Task> AddProduct;

    public ProductShowcaseControl()
    {
        _components = new System.ComponentModel.Container();

        InitializeComponent();
    }

    void IView.Show()
        => Show();

    private async void LoadAsync(object sender, EventArgs e)
    {
        ReadOnlyCollection<ProductModel>? products = await LoadProducts.Invoke();

        if (products == null)
            return;

        viewProductsTLP.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

        ThreadPool.QueueUserWorkItem((_) =>
        {
            viewProductsTLP.Invoke(async () =>
            {
                int countOfProducts = products.Count;

                int columnTPL = viewProductsTLP.ColumnCount;
                int rowTPL = countOfProducts / columnTPL;
                viewProductsTLP.RowCount = rowTPL;

                viewProductsTLP.SuspendLayout();
                for (int index = 0; index < countOfProducts; index++)
                {
                    int column = index % columnTPL;
                    int row = index / columnTPL;

                    ProductModel currentProduct = products[index];

                    ProductControl productControl = new ProductControl(currentProduct);
                    productControl.Dock = DockStyle.Fill;
                    productControl.AddButtonClicked += (s, e) => AddProduct.Invoke(currentProduct);
                    await productControl.CreateProductViewAsync();

                    viewProductsTLP.Controls.Add(productControl, column, row);
                }

                TableLayoutRowStyleCollection rowStyles = viewProductsTLP.RowStyles;
                TableLayoutColumnStyleCollection columnStyles = viewProductsTLP.ColumnStyles;

                rowStyles.Clear();
                columnStyles.Clear();

                for (int rowIndex = 0; rowIndex < rowTPL; rowIndex++)
                    rowStyles.Add(new RowStyle() { SizeType = SizeType.Percent });
                for (int columnIndex = 0; columnIndex < columnTPL; columnIndex++)
                    columnStyles.Add(new ColumnStyle() { SizeType = SizeType.Percent });

                viewProductsTLP.ResumeLayout();
            });
        });
    }

    private void BtnUpdate_Click(object sender, EventArgs e)
        => LoadAsync(sender, e);

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(searchTextBox.Text))
        {
            errorProvider.SetError(searchTextBox, "Введите название товара");
            return;
        }

        Control? needControl = viewProductsTLP.Controls.OfType<ProductControl>().FirstOrDefault(p => searchTextBox.Text.Equals(p.Tag?.ToString(), StringComparison.CurrentCultureIgnoreCase));

        if (needControl == null)
        {
            MessageBox.Show("Элемент не найден");
            return;
        }

        int desiredRow = viewProductsTLP.GetRow(needControl);
        int desiredColumn = viewProductsTLP.GetColumn(needControl);
        viewProductsTLP.ScrollControlIntoView(viewProductsTLP.GetControlFromPosition(desiredColumn, desiredRow));
        needControl.Select();
    }

    void IView.ShowMessage(string message, string caption, MessageLevel level)
        => MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, level switch
        {
            MessageLevel.Info => MessageBoxIcon.Information,
            MessageLevel.Warning => MessageBoxIcon.Warning,
            MessageLevel.Error => MessageBoxIcon.Error,
            _ => MessageBoxIcon.None,
        });

    void IView.Close()
       => Parent?.Controls.Remove(this);
}