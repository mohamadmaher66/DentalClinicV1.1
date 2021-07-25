import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { AlertService } from '../../service/alert.service';
import { Alert } from '../../models/alert.entity';
import { AlertType } from '../../enum/alert-type.enum';
import { HttpService } from '../../service/http-service';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.css']
})
export class FileUploaderComponent implements OnInit {

  public progress: number;
  public message: string;
  public isUploaded: boolean;
  public imgURLList = new Array<string>();

  @Input() public uploadURL: string;
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private httpService: HttpService,
    private alertService: AlertService) { }

  ngOnInit() {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    files.forEach(file => {
      if(!file.type.includes("image")){
        this.alertService.viewAlerts([{ title:"خطأ", message:"يجب اخيتار صورة", type: AlertType.Error}]);
        return;
      }
      let fileToUpload = <File>file;
      const formData = new FormData();
      formData.append('file', fileToUpload, fileToUpload.name);

      this.httpService.uploadFile(this.uploadURL, formData, true, 'events')
        .subscribe(
          event => this.uploadOnSuccess(event),
          err => this.uploadOnError(err)
        );
    });

  }

  private uploadOnSuccess(event: any) {
    if (event.type === HttpEventType.UploadProgress) {
      this.progress = Math.round(100 * event.loaded / event.total);
    }
    else if (event.type === HttpEventType.Response) {
      let alerts = new Array<Alert>();
      alerts.push({ type: AlertType.Info, title: "ملاحظة", message: "تم رفع الصور بنجاح" })
      this.alertService.viewAlerts(alerts);
      this.isUploaded = true;
      this.imgURLList.push(event.body);
      this.onUploadFinished.emit(event.body);
    }
  }

  private uploadOnError(error: any) {
    this.alertService.viewAlerts(error.error.alerts);
    this.isUploaded = false;
  }
}

