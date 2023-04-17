import { Observable } from 'rxjs';

/**
 * BaseNewsLetterService is an abstract class that provides an interface for checking an email address.
 * @abstract
 */
export abstract class BaseNewsLetterService {
    /**
     * checkEmail is an abstract method that takes an email address and returns an Observable of a boolean value.
     * The boolean value indicates if the email address is valid.
     * @abstract
     * @param {string} email - The email address to be checked.
     * @returns {Observable<boolean>} - An Observable of a boolean value that indicates if the email address is valid.
     */
    abstract checkEmail(email: string): Observable<boolean>;
}
