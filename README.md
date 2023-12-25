![Alt text](https://raw.githubusercontent.com/ETDA/e-TaxInvoice-PDFgen/master/eTaxInvoicePdfGenerator/icon_AppETax.png)


## e-Tax Invoice by TeDA - PDF Generator on PC 

สพธอ.ได้ พัฒนาโปรแกรมสร้างใบกํากับภาษีในรูปแบบ [PDF/A-3](https://en.wikipedia.org/wiki/PDF/A) ให้มีข้อมูล XML ตามเอกสารข้อเสนอแนะมาตรฐานด้านเทคโนโลยีสารสนเทศ และการสื่อสารที่จําเป็นต่อธุรกรรม ทางอิเล็กทรอนิกส์ [(ขมธอ. 3-2560 เวอร์ชั่น 2.0)](https://standard.etda.or.th/wp-content/uploads/2017/07/20161221-ER-EINVOICEV2-V08-16F-0706.pdf) โดยตัวโปรแกรมนี้จะทํางานบน Stand-alone PC

**คุณลักษณะสำคัญของ e-Tax Invoice by TeDA - PDF Generator on PC  :**
* บันทึกข้อมูลผู้ประกอบการ ทั้งผู้ขายและผู้ซื้อ
* บันทึกข้อมูลสินค้า/บริการ
* สร้างใบกำกับภาษีอิเล็กทรอนิกส์
* สร้างใบเพิ่มหนี้อิเล็กทรอนิกส์
* สร้างใบลดหนี้อิเล็กทรอนิกส์

## Attention Please
source code ของ e-Tax Invoice by TeDA - PDF Generator on PC นี้ เป็นเพียงตัวอย่างแนวทางสำหรับโปรแกรมจัดทำใบกำกับภาษีอิเล็กทรอนิกส์เท่านั้น ใบกำกับภาษีที่จัดทำขึ้นมาอาจจะมีบางจุดที่ไม่สอดคล้องกับข้อกำหนดปัจจุบันของกรมสรรพากร

| Project Status  | Master Branch |
| --------------- | ------------- |

| [![Build status](https://ci.appveyor.com/api/projects/status/p3jsldv9m6ijqcg7?svg=true)](https://ci.appveyor.com/project/jitti-etda/e-taxinvoice-pdfgen)
  | [![Build status](https://ci.appveyor.com/api/projects/status/p3jsldv9m6ijqcg7/branch/master?svg=true)](https://ci.appveyor.com/project/jitti-etda/e-taxinvoice-pdfgen/branch/master) |

## Latest Release

Download version ล่าสุดได้ที่นี่ [release page](https://github.com/ETDA/e-TaxInvoice-PDFgen/releases)

## Changelog 

[Read full changelog](https://github.com/ETDA/e-TaxInvoice-PDFgen/blob/master/CHANGELOG.md)
[1.1.0] - 2022-01-31

**Fix** 
- fix  แก้ไขชื่ออำเภอ

[1.0.6] - 2019-05-13

**Fix** 
- fix #29  หัวใบกำกับภาษีหายกรณีสร้างใบยกเลิกมากกว่าสองใบ

[1.0.5] - 2018-10-05

**Update** 
- update
  1. อัพเดทกระบวนการ read/write database เมื่อมีการ uninstall และ install ข้อมูลลูกค้าจะยังอยู่  
      *หมายเหตุ หากใช้ Installation version 1.0.4 ลงไปจะต้อง run program สำหรับอัพเกรด database เป็น version ใหม่ 
  2. รองรับใบเสร็จรับเงิน/ใบกำกับภาษี
  3. ปรับเพิ่มการแสดงผลความยาวของชื่อลูกค้า
  4. รองรับการใส่เครื่องหมาย &  < > ' "  
  5. ปรับล๊อคความยาวของ บ้านเลขที่ 
  6. รองรับผู้ซื้อที่เป็นชาวต่างชาติ โดยใช้ เลขที่หนังสือเดินทาง 
  7. รองรับผู้ซื้อที่เป็นคนไทย โดยใช้เลขที่บัตรประชาชน 

[1.0.4] - 2018-09-17

**Update** 
- update แขวงใหม่ ได้แก่ 
  1. เขตสะพานสูง - แขวงราษฎร์พัฒนา แขวงทับช้าง
  2. เขตพญาไท - แขวงพญาไท
  3. เขตดินแดน - แขวงรัชดาภิเษก
  4. เขตพระโขนง - แขวงพระโขนงใต้
  5. เขตสวนหลวง - แขวงอ่อนนุช แขวงพัฒนาการ
  6. เขตบางนา - แขวงบางนาเหนือ แขวงบางนาใต้
  7. เขตบางบอน - แขวงบางบอนเหนือ แขวงบางบอนใต้ แขวงคลองบางพราน และแขวงคลองบางบอน

## Contact Us
สามารถติดต่อเราได้ที่ eservice@etda.or.th หรือ Support-center: โทร 0 2026 6933

## Document
สามารถ download ได้จาก [ คู่มือโปรแกรมจัดเตรียมใบกำกับภาษีอิเล็กทรอนิกส์ในรูปแบบ PDF/A-3 (PC) ](https://etax.teda.th/etaxdocuments/eTaxInvoice_PDF_A3_pc.pdf)  

## Dependency
 * itextsharp (version 5.5.11)
 * itextsharp.pdfa (version 5.5.11)
 * itextsharp.xtra (version 5.5.11)
 * System.Data.SQLite.Core (version 1.0.105.2+)
 * Microsoft.ReportViewer.Common (version 10.0.40219.1+)
 * Microsoft.ReportViewer.WinForms (version 10.0.40219.1+)
 
## Font
 สามารถ download ได้จาก [THSarabun-PSK](https://github.com/ETDA/e-TaxInvoice-PDFgen/blob/master/Font/th-sarabun-psk.zip)
 
 > รายละเอียดการลง font สามารถอ่านได้จาก[ คู่มือโปรแกรมจัดเตรียมใบกำกับภาษีอิเล็กทรอนิกส์ในรูปแบบ PDF/A-3 (PC) ](https://etax.teda.th/etaxdocuments/eTaxInvoice_PDF_A3_pc.pdf)  

## License 
[GNU Affero General Public License v3.0](https://github.com/ETDA/e-TaxInvoice-PDFgen/blob/master/LICENSE)

Permissions of this strongest copyleft license are conditioned on making available complete source code of licensed works and modifications, which include larger works using a licensed work, under the same license. Copyright and license notices must be preserved. Contributors provide an express grant of patent rights. When a modified version is used to provide a service over a network, the complete source code of the modified version must be made available.


