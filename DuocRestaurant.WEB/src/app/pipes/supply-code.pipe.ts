import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'supplyCode'
})
export class SupplyCodePipe implements PipeTransform {

  transform(value: string, args?: any): string {
    if (!value)
      return null;

    return value.substring(0, 1) + "-" +
      value.substring(1, 6) + "-" +
      value.substring(5, 9) + "-" +
      value.substring(9);
  }

}
