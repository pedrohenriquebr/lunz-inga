import { Injectable } from "@angular/core";
import { BaseNewsLetterService } from "./newsletter.service";
import { HttpClient } from "@angular/common/http";
import { Observable, map, } from 'rxjs';
import { environment } from "src/environments/environment";
import { CheckEmailResponse } from "../models";

@Injectable()
export class NewsletterService implements BaseNewsLetterService {
    constructor(
        private http: HttpClient
    ) {
    }

    checkEmail(email: string): Observable<boolean> {
        return this.http.get<CheckEmailResponse>(environment.baseUrl + "/api/check-email/" + email)
            .pipe(
                map(x => x.exists)
            );
    }

}

