import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})

export class FileUploadComponent implements OnInit {
    public message: string;
    public errorMessage: string;
    public progress: number;
    public validationResult: any;

    @Output() public onUploadFInished = new EventEmitter();


    // Inject service 
    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit(): void {
        this.validationResult = { isFileValid: true, lineErrors: [] };
    }

    private clearFields() {
        this.message = "";
        this.validationResult = { isFileValid: true, lineErrors: [] };
        this.errorMessage = "";
    }

    public uploadFile = (files) => {
        if (files.length === 0) return;
        this.clearFields();
        let fileToUpload = <File>files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        this.http.post(location.origin + '/api/fileupload', formData, { reportProgress: true, observe: 'events' })
            .subscribe(
                (event) => {
                    if (event.type === HttpEventType.Response) {
                        this.message = event.statusText;
                        this.onUploadFInished.emit(event.body);
                    }
                },
                (error) => {
                    if (error.error.lineErrors) {
                        this.validationResult = error.error;
                    }
                    else {
                        this.errorMessage = error.error;
                    }
                }
            );
    }

}