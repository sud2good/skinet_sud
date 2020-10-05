import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketTotal } from '../../models/baskets';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit {
basketTotals$: Observable<IBasketTotal>;
  constructor(private basketService: BasketService ) { }

  ngOnInit(): void {
    this.basketTotals$ = this.basketService.basketTotal$;
  }

}
