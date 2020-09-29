import { Component, OnInit } from '@angular/core';
import{HttpClient} from '@angular/common/http';
import{Product} from '../../models/Product.model';


@Component({
  selector: 'app-cadastro',
  templateUrl: './cadastro.component.html',
  styleUrls: ['./cadastro.component.css']
})
export class CadastroComponent implements OnInit {
  
  public product: Product;
  
  constructor(private http: HttpClient) {}
  
  ngOnInit(): void {
    this.product= new Product();
    this.product.name="";
    this.product.value = null;
    this.product.codigo = "";
    this.product.description = "";
    this.product.quantity =null;
    this.getData();
  }
  
  getData(){
    
    let url="https://localhost:5001/Product/SelectAll";

    this.http.get(url).toPromise().then((data: any) => {
     
      console.log(data);
      this.product = new Product();

      this.product.name = data[0].name;
      this.product.value = data[0].value;
      this.product.codigo = data[0].codigo;
      this.product.description = data[0].description;
      this.product.quantity = data[0].quantity;
      
    //  console.log(JSON.stringify(data.json.name))
     // this.result=JSON.stringify(data.json.name)
    })
  }
}
