import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../shared/models/product';
import { Pagination } from '../shared/models/Pagination';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/ShopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
baseUrl='https://localhost:5001/api/';

shopParams = new ShopParams();
  constructor(private http: HttpClient) { }

  getProducts(brandId?:number,typeId?:number) {
    let params = new HttpParams();
    if (this.shopParams.brandId > 0) params = params.append('brandId', this.shopParams.brandId);
    if (this.shopParams.typeId) params = params.append('typeId', this.shopParams.typeId);
    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber);
    params = params.append('pageSize', this.shopParams.pageSize);
    if (this.shopParams.search) params = params.append('Search', this.shopParams.search);
    console.log("params=>",params.toString());
    return this.http.get<Pagination<Product[]>>(this.baseUrl+'products',{params});
  }
  getBrands() {
    return this.http.get<Brand[]>(this.baseUrl+'products/brands');
  }
  getTypes() {
    return this.http.get<Type[]>(this.baseUrl+'products/types');
  }
  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }
  getShopParams() {
    return this.shopParams;
  }
  
}
