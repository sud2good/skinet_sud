import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/baskets';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(private baksetService: BasketService, private checkoutService: CheckoutService, 
    private tostrService: ToastrService, private router: Router) { }

  ngOnInit(): void {
  }

  submitOrder() {
    const basket = this.baksetService.getCurrentBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) => {
      this.tostrService.success('Order created successfully');
      this.baksetService.deleteLocalBasket(basket.id);
      const navigationExtras: NavigationExtras = {state: order};
      this.router.navigate(['checkout/success'], navigationExtras); 
      console.log(order);
    }, error => {
      this.tostrService.error(error.message);
      console.log(error);
     });
  }

  getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    }
  }

}
