import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/Product.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  public product: Product;

  constructor(private http: HttpClient) { }

  getData() {

    let url = "https://localhost:5001/Product/SelectAll";

    this.http.get(url).toPromise().then((data: any) => {

      console.log(data);

      this.product.name = data[0].name;
      this.product.value = data[0].value;
      this.product.codigo = data[0].codigo;
      this.product.description = data[0].description;
      this.product.quantity = data[0].quantity;

      //pegando os valores do banco de dados  
      //console.log(JSON.stringify(data.json.name))
      //this.result=JSON.stringify(data.json.name)
    })

  }

  ngOnInit() {
    //metodo que inicia a componente.
    this.product= new Product();
    this.product.name = "";

  }


}
