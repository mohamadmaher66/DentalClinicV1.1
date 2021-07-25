import { Injectable, EventEmitter } from '@angular/core';
import { throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SessionService } from './session.service';
import { ToastrService } from 'ngx-toastr';
import { hasValue } from './helper.service';

@Injectable()
export class HttpService {

    private baseUrl: string;
    inProgressEventEmitter: EventEmitter<boolean> = new EventEmitter();

    constructor(private http: HttpClient,
        private configService: ConfigService,
        private sessionService: SessionService,
        private toastr: ToastrService) {
    }

    public httpPost(data: any, url: string) {
        this.inProgressEventEmitterChange(true);
        this.baseUrl = this.configService.configuration.apiUrl;
        url = this.baseUrl + url;

        let body = JSON.stringify(data);
        let headers = new HttpHeaders()
            .set('Content-Type', 'application/json; charset=utf-8')
            .set('Accept', 'q=0.8;application/json;q=0.9');

        if (typeof (Storage) !== "undefined" && hasValue(this.sessionService.getToken())) {
            headers = headers.set('Authorization', this.sessionService.getToken());
        }

        let options = { headers: headers, body: body };
        return this.http.post(url, body, options)
            .pipe(map((response) => this.parseResponse(response)))
            .pipe(catchError(err => this.handleError(err)));
    }

    public httpDownloadFile(data: any, url: string) {
        this.inProgressEventEmitterChange(true);
        this.baseUrl = this.configService.configuration.apiUrl;
        url = this.baseUrl + url;

        let body = JSON.stringify(data);
        let headers = new HttpHeaders()
            .set('Content-Type', 'application/json; charset=utf-8')
            .set('Accept', 'q=0.8;application/json;q=0.9');

        if (typeof (Storage) !== "undefined" && hasValue(this.sessionService.getToken())) {
            headers = headers.set('Authorization', this.sessionService.getToken());
        }

        let options = { headers: headers, body: body };
        return this.http.post(url, body, { headers, responseType: 'blob' })
            .pipe(map((response) => this.parseResponse(response)))
            .pipe(catchError(err => this.handleError(err)));
    }

    private parseResponse(response: any) {
        this.inProgressEventEmitterChange(false);
        return response;
    }

    public uploadFile(uploadURL: string, formData: FormData, reportProgress: boolean, observe: string) {
        this.inProgressEventEmitterChange(true);

        let headers = new HttpHeaders()

        if (typeof (Storage) !== "undefined" && hasValue(this.sessionService.getToken())) {
            headers = headers.set('Authorization', this.sessionService.getToken());
        }

        return this.http.post(uploadURL, formData, { headers, reportProgress: true, observe: 'events' })
            .pipe(map((response) => this.parseResponse(response)))
            .pipe(catchError(err => this.handleError(err)));
    }

    private handleError(error: any) {
        this.inProgressEventEmitterChange(false);
        let body = '';

        switch (error.status) {
            case 401:
            case 408:
            case 307:
            case 407:
                this.sessionService.signOutWithErrorMessage(null);
                body = error;
                break;

            case 0:
                this.toastr.error("Connection Failed", "Error");
                break;

            default:
                body = error;
                break;
        }
        return throwError(body);
    }

    public inProgressEventEmitterChange(inProgress: boolean) {
        this.inProgressEventEmitter.emit(inProgress);
    }
}
