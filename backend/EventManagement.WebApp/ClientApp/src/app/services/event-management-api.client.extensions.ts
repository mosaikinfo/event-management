import { AuthService } from './auth.service';

export class ServiceBase {
    
    constructor(private authService: AuthService) {}

    protected transformOptions(options: any) {
        options.headers = {
            'Content-Type':  'application/json',
            'Authorization': this.authService.getAuthorizationHeaderValue()
        };
        return Promise.resolve(options);
    }
}