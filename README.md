# PCRBattleRecorder

Scripts for Princess Connect Re:Dive (CSharp Ver)

## 简介

**PCRBattleRecorder:** 基于OpenCv和TesseractOCR以及Mumu模拟器的公主连结脚本，目前只搭建了大概的框架

## 下载

[1.0.0beta](https://github.com/dreamlive0815/PCRBattleRecorder/blob/master/Publish/PCRBattleRecorder.rar?raw=true)点此下载  
PCRData文件夹最好手动与项目同步  
Tesseract则一般不需要更新

## 使用

本程序存在三个主要的外部依赖资源，这三个资源所在的文件夹不需要和脚本主程序同级

- **PCRData**：存放PCR解包资源以及一些程序使用的配置文件，可以从本项目PCRData文件夹取用，也可以从[这里](https://github.com/dreamlive0815/PCRBattleRecorder/blob/master/Publish/PCRData.rar?raw=true)下载
- **Tesseract**：[Tesseract-OCR](https://github.com/tesseract-ocr/tesseract)，谷歌旗下的开源识图程序，本程序使用Shell方式调用Tesseract，因此需要指定Tesseract主程序位置，可以去[这里](https://share.weiyun.com/Aupe71lq)下载
- **AdbServer**: Mumu模拟器adb_server程序，这个程序一般在Mumu安装目录的相对目录emulator/nemu/vmonitor/bin下

## 特别注意

本程序默认Mumu使用2160x1080分辨率，DPI320，如果不是，请到Mumu设置中心->界面设置中进行修改并重启。  
为保证最好的运行效果，请使用本程序菜单脚本->跟踪Mumu窗口大小功能，并手动调整Mumu窗口大小使Mumu窗口大小接近1280x640（Size: 1280,640）。  
本程序运行期间请保证没用其他窗口覆盖在Mumu窗口之上（也就是最好将Mumu窗口置顶选项勾上），并且不要将Mumu移到到屏幕视口之外  

## 汇报Bug

如果在使用过程中遇到了BUG，你可以通过995928339@qq.com这个邮箱联系到我，并将主程序相对路径log/error.log这个错误日志文件发送给我

## 友情链接

**干炸里脊资源站**: https://redive.estertion.win/

