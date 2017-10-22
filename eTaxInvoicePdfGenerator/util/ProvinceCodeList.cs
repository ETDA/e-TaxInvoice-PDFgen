using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using eTaxInvoicePdfGenerator.Dao;
using eTaxInvoicePdfGenerator.Entity;

namespace eTaxInvoicePdfGenerator.util
{
    class ProvinceCodeList
    {
        internal void SetProvince(ComboBox comboBox)
        {
            List<AddressCodeListObj> list = new AddressCodeListDao().ProvinceList();
            comboBox.DisplayMemberPath = "changwat_th";
            comboBox.SelectedValuePath = "code";
            comboBox.ItemsSource = list;
            comboBox.SelectedIndex = 0;
        }

        internal void SetDistrict(ComboBox comboBox, string code)
        {
            List<AddressCodeListObj> list = new AddressCodeListDao().DistrictList(code);
            comboBox.DisplayMemberPath = "amphoe_th";
            comboBox.SelectedValuePath = "code";
            comboBox.ItemsSource = list;
            comboBox.SelectedIndex = 0;
        }

        internal void SetSubDistrict(ComboBox comboBox, string code)
        {
            List<AddressCodeListObj> list = new AddressCodeListDao().SubDistrictList(code);
            comboBox.DisplayMemberPath = "tambon_th";
            comboBox.SelectedValuePath = "code";
            comboBox.ItemsSource = list;
            comboBox.SelectedIndex = 0;
        }
    }
}
