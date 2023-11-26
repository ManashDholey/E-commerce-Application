import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit{
  product?: Product;
  quantity = 1;
  quantityInBasket = 0;
  constructor(private shopService: ShopService , private activateRoute: ActivatedRoute){}
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  loadProduct(){
    const id=this.activateRoute.snapshot.paramMap.get('id');
    if(id)this.shopService.getProduct(+id).subscribe({
      next: response => this.product=response,
      error: error=>console.error(error),
    });
  }
  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    this.quantity--;
  }
}
