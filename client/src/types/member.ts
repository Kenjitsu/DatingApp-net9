export type Member = {
    id: string
    displayName: string
    dateOfBirth: string
    imageUrl?: string
    created: string
    lastActive: string
    gender: string
    description?: string
    city: string
    country: string
}

export type EditableMember = {
    displayName: string;
    description?: string;
    city: string;
    country: string;
}

