import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Pagination } from './shared/models/Pagination';
import { Product } from './shared/models/product';
import { BasketService } from './basket/basket.service';
import { AccountService } from './account/account.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  
  title = 'Ecommerce Shop';
  products : any[] =[];
  PageIndex=1;
  PageSize=10;
  Sort="priceAsc";
  Search="Core";
constructor(private accountService: AccountService,private basketService: BasketService){}

  ngOnInit(): void {
//     try{
//     this.http.get<Pagination<Product[]>>(`https://localhost:5001/api/Products?PageIndex=${this.PageIndex}&PageSize=${this.PageSize}&Sort=${this.Sort}&Search=${this.Search}`).subscribe({
//       next: response =>{ console.log(response);
//       this.products=response.data},
//       error: error=> console.log(error),
//       complete:()=>{
//         console.log('request completed');
//         console.log('extra statement.');
//       }
//     })
//   }catch(e){
// console.log(e);
//   }
this.loadBasket();
  }
  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) this.basketService.getBasket(basketId);
  }
  loadCurrentUser(){
    const token= localStorage.getItem('token');
    if(token){
      this.accountService.loadCurrentUser(token).subscribe();
    } 
  }
}
