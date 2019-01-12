import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { EventManagementApiClient, WeatherForecast } from '../services/event-management-api.client';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private authService: AuthService,
    private apiClient: EventManagementApiClient)
  {
    this.apiClient.sampleData_WeatherForecasts()
      .subscribe(result => {
        this.forecasts = result;
      }, error => console.error(error));
  }
}