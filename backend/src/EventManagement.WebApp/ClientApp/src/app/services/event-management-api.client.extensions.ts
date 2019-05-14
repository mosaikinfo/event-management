import { AuthService } from './auth.service';

export class ServiceBase {
    
    constructor(private authService: AuthService) {}

    protected transformOptions(options: any) {
        return this.authService.getAuthorizationHeaderValue().then(headerValue => {
            options.headers = {
                'Content-Type':  'application/json',
                'Authorization': headerValue
            };
            return options;
        });
    }
}