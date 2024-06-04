using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;
using System.Collections.ObjectModel;

namespace OnlineStore.MVP.Views.Implementations.MainWindow.Sections;

public sealed partial class ShoppingCartControl : UserControl, IShoppingCartView
{
    private readonly List<ProductModel> _products;

    public event Func<ICollection<ProductModel>, Task> Order;
    public event Func<Task<ReadOnlyCollection<OrderModel>?>> LoadOrders;

    public ShoppingCartControl()
    {
        _components = new System.ComponentModel.Container();

        InitializeComponent();

        viewProductsTLP.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

        _products = [];
    }

    void IView.Show()
        => Show();

    private async void LoadOrderProducts(object sender, EventArgs e)
    {
        orderButton.Enabled = false;
        ReadOnlyCollection<OrderModel>? orders = await LoadOrders.Invoke();

        if (orders == null)
            return;

        ThreadPool.QueueUserWorkItem((_) =>
        {
            viewProductsTLP.Invoke(async () =>
            {
                int countOfOrders = orders.Count;

                int columnTPL = viewProductsTLP.ColumnCount;
                int rowTPL = countOfOrders / columnTPL;
                viewProductsTLP.RowCount = rowTPL;

                viewProductsTLP.SuspendLayout();
                for (int orderIndex = 0; orderIndex < countOfOrders; orderIndex++)
                {
                    int column = orderIndex % columnTPL;
                    int row = orderIndex / columnTPL;

                    OrderModel currentOrder = orders[orderIndex];
                    ProductModel[] products = [.. currentOrder.Products];
                    for (int productIndex = 0; productIndex < products.Length; productIndex++)
                    {
                        ProductModel currentProduct = products[productIndex];

                        ProductControl productControl = new ProductControl(currentProduct);
                        productControl.Dock = DockStyle.Fill;
                        productControl.DeleteButtonClicked += (s, e) =>
                        {
                            ProductControl? product = viewProductsTLP.Controls
                                                                .OfType<ProductControl>()
                                                                .FirstOrDefault(p => p.Tag?.ToString() == currentProduct.Name);

                            if (product != null)
                                viewProductsTLP.Controls.Remove(product);
                        };
                        await productControl.CreateProductViewAsync();

                        viewProductsTLP.Controls.Add(productControl, column, row);
                    }
                }
                viewProductsTLP.ResumeLayout();

                TableLayoutRowStyleCollection rowStyles = viewProductsTLP.RowStyles;
                TableLayoutColumnStyleCollection columnStyles = viewProductsTLP.ColumnStyles;

                rowStyles.Clear();
                columnStyles.Clear();

                for (int rowIndex = 0; rowIndex < rowTPL; rowIndex++)
                    rowStyles.Add(new RowStyle() { SizeType = SizeType.Percent });
                for (int columnIndex = 0; columnIndex < columnTPL; columnIndex++)
                    columnStyles.Add(new ColumnStyle() { SizeType = SizeType.Percent });

                viewProductsTLP.Invalidate();
            });
        });

        orderButton.Enabled = true;
    }

    private async void BtnOrder_Click(object sender, EventArgs e)
        => await Order.Invoke(_products);

    void IShoppingCartView.AddProduct(ProductModel product)
    {
        async void Action()
        {
            ProductControl productControl = new ProductControl(product);
            productControl.Dock = DockStyle.Fill;
            productControl.DeleteButtonClicked += (s, e) =>
            {
                ProductControl? findProduct = viewProductsTLP.Controls
                                                    .OfType<ProductControl>()
                                                    .FirstOrDefault(p => p.Tag?.ToString() == product.Name);

                if (findProduct != null)
                {
                    viewProductsTLP.Controls.Remove(findProduct);
                    _products.Remove(product);
                }
            };

            await productControl.CreateProductViewAsync();

            _products.Add(product);
            viewProductsTLP.Controls.Add(productControl);
        }

        if (viewProductsTLP.InvokeRequired)
            viewProductsTLP.Invoke(Action);
        else
            Action();
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(searchTextBox.Text))
        {
            errorProvider.SetError(searchTextBox, "Введите название товара");
            return;
        }

        Control? needControl = viewProductsTLP.Controls.OfType<ProductControl>()
                                                       .FirstOrDefault(p => searchTextBox.Text.Equals(p.Tag?.ToString(), StringComparison.CurrentCultureIgnoreCase));

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