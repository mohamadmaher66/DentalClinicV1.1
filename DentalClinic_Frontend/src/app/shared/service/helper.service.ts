export function hasValue(obj: any) {
    if (obj != undefined && obj != null) {
        return true;
    }
    return false;
}

export function listHasValue<T>(list: Array<T>){
    if(list != undefined && list.length > 0){
        return true;
    }
    return false;
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

export function cloneObject<T>(obj: T): T {
    const cache = new Set();
    return JSON.parse(JSON.stringify(obj, function (key, value) {
        if (typeof value === 'object' && value !== null) {
            if (cache.has(value)) {
                // Circular reference found
                try {
                    // If this value does not reference a parent it can be deduped
                    return JSON.parse(JSON.stringify(value));
                }
                catch (err) {
                    // discard key if value cannot be deduped
                    return;
                }
            }
            // Store value in our set
            cache.add(value);
        }
        return value;
    }));
}

export function parseToLocalDate(date: any) {
    if (date instanceof Date) return date;
    var isUtc = /Z|[\+\-]\d\d:?\d\d/i.test(date);
    if (!isUtc) {
        date += "Z";
    }
    return new Date(date);
};

export function isNullOrEmptyString(value: string): boolean {
    if (hasValue(value) && value.length > 0) {
        return false;
    }
    return true;
}
