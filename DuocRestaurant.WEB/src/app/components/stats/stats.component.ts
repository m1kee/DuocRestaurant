import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { StatsService } from '@services/stats.service';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css']
})
export class StatsComponent implements OnInit {
  monthlyChart: Chart 

  showMonthlyChart: boolean = false;

  constructor(private statsService: StatsService) { }

  ngOnInit(): void {
    this.statsService.getMonthlySells().subscribe((response: any) => {
      if (response) {
        this.monthlyChart = new Chart({
          chart: {
            type: 'column'
          },
          title: {
            text: 'Ventas del mes'
          },
          xAxis: {
            categories: response.Categories,
            crosshair: true
          },
          yAxis: {
            min: 0,
            title: {
              text: ''
            }
          },
          tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
              '<td style="padding:0"><b>${point.y}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
          },
          plotOptions: {
            column: {
              pointPadding: 0.2,
              borderWidth: 0
            }
          },
          series: [response.Serie]
        });
        this.showMonthlyChart = true;
      }
    }, (error) => { this.showMonthlyChart = false; });
  }
}
