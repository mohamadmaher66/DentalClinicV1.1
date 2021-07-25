import { Alert } from './alert.entity';
import { DetailsList } from './details-list.entity';
import { GridSettings } from './grid-settings.entity';

export class RequestedData<T> {
    entity: T;
    entityList: Array<T>;
    detailsList: Array<DetailsList>;
    gridSettings: GridSettings;
    alerts = new Array<Alert>();
}
