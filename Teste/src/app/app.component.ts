import { Component } from '@angular/core';
import{HttpClient} from '@angular/common/http';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html', 
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
 

  name: string;
  result: string;

  constructor(private http:HttpClient){}

  postData(){
    
    let url="http://localhost:5000/Product/SelectAll";

    this.http.get(url)
    
    .toPromise().then((data: any) => {
      console.log(data)
      console.log(JSON.stringify(data.json.name))
      this.result=JSON.stringify(data.json.name)
    })
  }
}
