import { Observable } from 'rxjs';

export abstract class BaseNewsLetterService {
    abstract checkEmail(email: string): Observable<boolean>;
}
