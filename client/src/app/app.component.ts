import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Skinet';
  // products: IProduct[];

  // constructor(private http: HttpClient){
    constructor(private basketService: BasketService){
  }

ngOnInit(): void {
    // this.http.get('https://localhost:5001/api/products?pageSize=50').subscribe(
    //   (response: IPagination) => {
    //   // console.log(response);
    //   this.products = response.data;
    // }, error => {
    //   console.log(error);
    // });
  const basketId = localStorage.getItem('basket_id');
  if (basketId) {
    this.basketService.getBasket(basketId).subscribe(() => {
      console.log('basket Initialized');
    }, error => {
      console.log('error');
    }
    );
  }
}
}
