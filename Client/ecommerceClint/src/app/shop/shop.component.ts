import { Component, OnInit } from '@angular/core';
import { ShopService } from './shop.service';
import { Product } from '../shared/models/product';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/ShopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products : Product[] = [];
  brands: Brand[] = [];
  types:Type[] = [];
  shopParams: ShopParams;
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to high', value: 'priceAsc'},
    {name: 'Price: High to low', value: 'priceDesc'},
  ];
  totalCount = 0;
  PageIndex=1;
PageSize=10;
Sort="priceAsc";
Search="Core";
brandIdSelected=0;
typeIdSelected=0;
  constructor(private shopService: ShopService){this.shopParams = shopService.getShopParams();}
  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }
  getProducts() {
    // this.shopParams.pageNumber=this.PageIndex;
    // this.shopParams.pageSize=this.PageSize;
    // this.shopParams.sort=this.Sort;
     this.shopParams.search=this.Search; 
    this.shopService.getProducts(this.brandIdSelected,this.typeIdSelected).subscribe({
      next: (response) => { this.products = response.data.flat();
        console.log("this.products===>",this.products);},
      error: error => console.log(error),
    })
  }
  onSortSelected(event: any) {
    const params = this.shopService.getShopParams();
    params.sort = event.target.value;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }
  getBrands() {
    this.shopService.getBrands().subscribe({
      next: response => this.brands = [{id:0,name:'All'},...response],
      error: error => console.log(error),
    })
  }
  getTypes() {
    this.shopService.getTypes().subscribe({
      next:(response :Type[])  => this.types = [{id:0,name:'All'},...response],
      error: (error :any)=> console.log(error),
    })
  }
  onBrandSelected(brandId:number){
    this.brandIdSelected=brandId;
    this.getProducts();
  }
  onTypeSelected(typeId:number){
    this.typeIdSelected=typeId;
    this.getProducts();
  }
  
}
