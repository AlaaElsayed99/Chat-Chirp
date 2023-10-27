import { HttpClient, HttpParams } from "@angular/common/http";
import { PaginationResult } from "../_models/Pagination";
import { map } from "rxjs";


export function  getPagintedResult<T>(url:string,params:HttpParams,http:HttpClient){
    const paginatedResult:PaginationResult<T>=new PaginationResult<T>
  return http.get<T>(url, {observe:'response',params}).pipe(
      map(resopne=> {
        if(resopne.body){
          paginatedResult.result=resopne.body;
  
        }
        const pagination= resopne.headers.get('pagination');
        if(pagination){
          paginatedResult.pagination=JSON.parse(pagination);
        }
        return paginatedResult;
      })
  )}
  
 export function  getPaginationHeaders(pageNumber:number,pageSize:number) {
    let params = new HttpParams();
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
  
    return params;
  }
  