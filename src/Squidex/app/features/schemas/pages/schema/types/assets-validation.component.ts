/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Sebastian Stehle. All rights reserved
 */

import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { AssetsFieldPropertiesDto } from 'shared';

@Component({
    selector: 'sqx-assets-validation',
    styleUrls: ['assets-validation.component.scss'],
    templateUrl: 'assets-validation.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AssetsValidationComponent implements OnInit {
    @Input()
    public editForm: FormGroup;

    @Input()
    public properties: AssetsFieldPropertiesDto;

    public ngOnInit() {
        this.editForm.setControl('minItems',
            new FormControl(this.properties.minItems));

        this.editForm.setControl('maxItems',
            new FormControl(this.properties.maxItems));

        this.editForm.setControl('minSize',
            new FormControl(this.properties.minSize));

        this.editForm.setControl('maxSize',
            new FormControl(this.properties.maxSize));

        this.editForm.setControl('allowedExtensions',
            new FormControl(this.properties.allowedExtensions));

        this.editForm.setControl('mustBeImage',
            new FormControl(this.properties.mustBeImage));

        this.editForm.setControl('minWidth',
            new FormControl(this.properties.minWidth));

        this.editForm.setControl('maxWidth',
            new FormControl(this.properties.maxWidth));

        this.editForm.setControl('minHeight',
            new FormControl(this.properties.minHeight));

        this.editForm.setControl('maxHeight',
            new FormControl(this.properties.maxHeight));

        this.editForm.setControl('aspectWidth',
            new FormControl(this.properties.aspectWidth));

        this.editForm.setControl('aspectHeight',
            new FormControl(this.properties.aspectHeight));
    }
}