import { Component } from '@angular/core';
import { HomePage } from '@core/home.page';

@Component({
  selector: 'app-root',
  imports: [HomePage],
  host: {
    class: 'app-root',
  },
  template: ` <home-page></home-page> `,
  styles: [
    `
      :host {
        display: block;
        width: 100%;
        height: 100%;
      }
    `,
  ],
})
export class App {}
