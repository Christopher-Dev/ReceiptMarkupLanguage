using ReceiptBuilder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlEditorWeb.ViewModel
{
    public interface IReceiptViewModel
    {
        List<ReceiptModel> Receipts { get; set; }
        ReceiptModel? SelectedReceipt { get; set; }

        event EventHandler? HomeButtonPressed;
        event EventHandler? CancelButtonPressed;
        event EventHandler<bool>? ValidationResult;
        event EventHandler? SettingsButtonPressed;
        event EventHandler? ClearCodeButtonPressed;

        Task UpdateValidationAsync(bool validationResult);
        Task EncodeImage();

        void OnHomeButtonPressed();
        void OnCancelButtonPressed();
        void OnSettingsButtonPressed();
        void OnClearCodeButtonPressed();
    }

    public class ReceiptViewModel : IReceiptViewModel
    {
        public List<ReceiptModel> Receipts { get; set; } = new List<ReceiptModel>();
        public ReceiptModel? SelectedReceipt { get; set; }

        public event EventHandler? HomeButtonPressed;
        public event EventHandler? CancelButtonPressed;
        public event EventHandler<bool>? ValidationResult;
        public event EventHandler? SettingsButtonPressed;
        public event EventHandler? ClearCodeButtonPressed;

        public ReceiptViewModel()
        {
        }

        public async Task UpdateValidationAsync(bool validationResult)
        {
            ValidationResult?.Invoke(this, validationResult);
        }

        public async Task EncodeImage()
        {
            // Implementation of image encoding
        }

        public void OnHomeButtonPressed()
        {
            HomeButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        public void OnCancelButtonPressed()
        {
            CancelButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        public void OnSettingsButtonPressed()
        {
            SettingsButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        public void OnClearCodeButtonPressed()
        {
            ClearCodeButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }

}
