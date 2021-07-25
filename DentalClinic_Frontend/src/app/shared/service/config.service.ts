import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { getBaseUrl } from './helper.service';
import { Configuration } from '../models/configuration.entity';

@Injectable()
export class ConfigService {

    public configuration: Configuration;

    constructor(private http: HttpClient) {
        this.checkConfiguration(false);
    }

    public async checkConfiguration(isReset: boolean) {
        if (isReset) {
            this.configuration.apiUrl = "";
        }
        await this.getConfiguration();
        localStorage.setItem("pageSize", this.configuration.pageSize.toString());
    }

    private async getConfiguration(): Promise<any> {
        if (this.configuration == null || this.configuration.apiUrl.length == 0) {
            this.configuration = (await this.http.get(getBaseUrl() + '/config.json').toPromise() as Configuration);
        }
        return Promise.resolve(this.configuration);
    }
}